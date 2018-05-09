using System;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Data;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Services;
using SFA.DAS.PSRService.Web.StartupConfiguration;
using StructureMap;
using ConfigurationService = SFA.DAS.PSRService.Web.Services.ConfigurationService;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace SFA.DAS.PSRService.Web
{
    public class Startup
    {
        private const string ServiceName = "SFA.DAS.PSRService";
        private const string Version = "1.0";
        private IHostingEnvironment _hostingEnvironment;

        public Startup(IConfiguration config, IHostingEnvironment env)
        {
            Configuration = ConfigurationService.GetConfig(config["EnvironmentName"], config["ConfigurationStorageConnectionString"], Version, ServiceName).Result;

            _hostingEnvironment = env;
        }

        public IWebConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IEmployerAccountService, EmployerAccountService>();
            services.AddTransient<IAccountApiClient, AccountApiClient>();
            services.AddTransient<IAccountApiConfiguration, AccountApiConfiguration>();
            services.AddSingleton<IAccountApiConfiguration>(Configuration.AccountsApi);

            var sp = services.BuildServiceProvider();

            services.AddAndConfigureAuthentication(Configuration, sp.GetService<IEmployerAccountService>());
            services.AddAuthorizationService();
            services.AddMvc(opts=>opts.Filters.Add(new AuthorizeFilter("HasEmployerAccount")) ).AddControllersAsServices().AddSessionStateTempDataProvider();
            //services.AddMvc().AddControllersAsServices().AddSessionStateTempDataProvider();

           

            services.AddSession(config => config.IdleTimeout = TimeSpan.FromHours(1));

            //This makes sure all automapper profiles are automatically configured for use
            //Simply create a profile in code and this will register it
            services.AddAutoMapper();



            return ConfigureIOC(services);
        }

        private IServiceProvider ConfigureIOC(IServiceCollection services)
        {
            var container = new Container();

            container.Configure(config =>
            {
                config.Scan(_ =>
                {
                    _.AssemblyContainingType(typeof(Startup));
                    _.WithDefaultConventions();
                });

                //config.For<IHttpClient>().Use<StandardHttpClient>();
                //config.For<ICache>().Use<SessionCache>();
                //config.For<ITokenService>().Use<TokenService>();
                config.For<IWebConfiguration>().Use(Configuration);
                //config.For<IContactsApiClient>().Use<ContactsApiClient>().Ctor<string>().Is(Configuration.ClientApiAuthentication.ApiBaseAddress);
                config.For<IReportService>().Use<ReportService>();
                config.For<IReportRepository>().Use<SQLReportRepository>().Ctor<string>().Is(Configuration.SqlConnectionString);
                config.For<IEmployerAccountService>().Use<EmployerAccountService>();
                var physicalProvider = _hostingEnvironment.ContentRootFileProvider;
                config.For<IFileProvider>().Singleton().Use(physicalProvider);

                config.Populate(services);


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
            });

            return container.GetInstance<IServiceProvider>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles()
                .UseErrorLoggingMiddleware()
                .UseSession()
                .UseAuthentication()
                .UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "accounts/{employerAccountId}/{controller=Home}/{action=Index}/{id?}");
                });
        }

    }
}
