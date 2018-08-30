using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Application.Mapping;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Data;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.Services;
using StructureMap;
using ILogger = NLog.ILogger;

namespace SFA.DAS.PSRService.IntegrationTests
{
    internal static class TestHelper
    {
        public static string ConnectionString { get; }

        static TestHelper()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            ConnectionString = config["ConnectionString"];
        }

        public static Period CurrentPeriod => Period.FromInstantInPeriod(DateTime.UtcNow.Date);

        public static void CreateReport(ReportDto report)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute(@"
                    INSERT INTO [dbo].[Report] ([Id],[EmployerId],[ReportingPeriod],[ReportingData],[Submitted])
                                        VALUES (@Id, @EmployerId, @ReportingPeriod, @ReportingData, @Submitted)",
                    new {report.Id, report.EmployerId, report.ReportingData, report.ReportingPeriod, report.Submitted});
            }
        }

        public static IList<ReportDto> GetAllReports()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                return connection.Query<ReportDto>("select * from Report").ToList();
            }
        }

        public static void ClearData()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute("if exists(select 1 from Report) delete from Report");
            }

        }
        public static Action<ConfigurationExpression> ConfigureIoc()
        {
            return config =>
            {
                config.Scan(_ =>
                {
                    _.AssemblyContainingType(typeof(Startup));
                    _.WithDefaultConventions();
                });

                config.For<IWebConfiguration>().Use(new WebConfiguration
                {
                    SqlConnectionString = TestHelper.ConnectionString,
                });
                config.For<IReportService>().Use<ReportService>();
                config.For<IReportRepository>().Use<SQLReportRepository>().Ctor<string>().Is(TestHelper.ConnectionString);
                config.For<IEmployerAccountService>().Use<EmployerAccountService>();
                config.For<IFileProvider>().Singleton().Use(new PhysicalFileProvider(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));

                config.Scan(scanner =>
                {
                    scanner.AssemblyContainingType<GetReportRequest>(); // Our assembly with requests & handlers
                    scanner.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<>)); // Handlers with no response
                    scanner.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>)); // Handlers with a response
                    scanner.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>));
                });

                config.For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
                config.For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));
                config.For<IMediator>().Use<Mediator>();
                config.For(typeof(ILogger<UserService>)).Use(Mock.Of<ILogger<UserService>>());
                config.For<IAuthorizationService>().Use(Mock.Of<IAuthorizationService>());

                var mockEmployerAccountService = new Mock<IEmployerAccountService>();
                var employerIdentifier = new EmployerIdentifier {AccountId = "111", EmployerName = "222"};
                mockEmployerAccountService.Setup(e => e.GetCurrentEmployerAccountId(It.IsAny<HttpContext>())).Returns(employerIdentifier);
                config.For<IEmployerAccountService>().Use(mockEmployerAccountService.Object);
                
                var mapConfig = new MapperConfiguration(cfg => cfg.AddProfile<ReportMappingProfile>());
                var mapper = mapConfig.CreateMapper();
                config.For<IMapper>().Use(mapper);
            };
        }
    }
}
