using System.Collections.Generic;
using System.IO;
using System.Reflection;
using AutoMapper;
using Dapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Application.Mapping;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.IntegrationTests;

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
                new { report.Id, report.EmployerId, report.ReportingData, report.ReportingPeriod, report.Submitted });
        }
    }

    public static List<ReportDto> GetAllReports()
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

    public static IServiceCollection ConfigureTestServices(this IServiceCollection services)
    {
        services.AddSingleton<IWebConfiguration>(new WebConfiguration
        {
            SqlConnectionString = ConnectionString,
        });

        services.AddSingleton<IReportService, ReportService>();
        services.AddSingleton<IReportRepository>(new Data.SqlReportRepository(new SqlConnection(ConnectionString)));
        services.AddSingleton<IEmployerAccountService, EmployerAccountService>();
        services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));

        services.AddTransient<IMediator, Mediator>();
        services.AddTransient(_ => Mock.Of<ILogger<UserService>>());
        services.AddTransient(_ => Mock.Of<IAuthorizationService>());

        var mockEmployerAccountService = new Mock<IEmployerAccountService>();
        var employerIdentifier = new EmployerIdentifier { AccountId = "111", EmployerName = "222" };
        mockEmployerAccountService.Setup(e => e.GetCurrentEmployerAccountId(It.IsAny<HttpContext>())).Returns(employerIdentifier);
        services.AddTransient(_ => mockEmployerAccountService.Object);

        var mapConfig = new MapperConfiguration(cfg => cfg.AddProfile<ReportMappingProfile>());
        services.AddTransient(_ => mapConfig.CreateMapper());

        services.AddSingleton<HomeController>();
        services.AddSingleton<QuestionController>();
        services.AddSingleton<ReportController>();
        services.AddSingleton<ServiceController>();

        return services;
    }
}