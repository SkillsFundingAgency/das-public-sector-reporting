﻿using System;
using System.Linq.Expressions;
using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Web.ViewModels;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Data;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Configuration.Authorization;
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
            
   
            var constants = new Constants(Configuration.Identity);
            UserLinksViewModel.ChangePasswordLink = $"{constants.ChangePasswordLink()}{WebUtility.UrlEncode(Configuration.ApplicationUrl + "/service/changePassword")}";
            UserLinksViewModel.ChangeEmailLink = $"{constants.ChangeEmailLink()}{WebUtility.UrlEncode(Configuration.ApplicationUrl + "/service/changeEmail")}";



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
            services.AddMvc(opts=>opts.Filters.Add(new AuthorizeFilter(PolicyNames.HasEmployerAccount)) ).AddControllersAsServices().AddSessionStateTempDataProvider();
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
                    _.SingleImplementationsOfInterface();
                });

                config.For<IWebConfiguration>().Use(Configuration);
                config.For<IReportRepository>().Use<SQLReportRepository>().Ctor<string>().Is(Configuration.SqlConnectionString);
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
                    routes.MapRoute(
                        name: "Service-Controller",
                        template: "Service/{action}", 
                    defaults: new {controller = "Service"});

                });
        }

        public class Constants
        {
            private readonly IdentityServerConfiguration _configuration;

            public Constants(IdentityServerConfiguration configuration)
            {
                _configuration = configuration;
            }
            
            public string ChangeEmailLink() => _configuration.Authority.Replace("/identity", "") + string.Format(_configuration.ChangeEmailLink, _configuration.ClientId);
            public string ChangePasswordLink() => _configuration.Authority.Replace("/identity", "") + string.Format(_configuration.ChangePasswordLink, _configuration.ClientId);
            
        }
    }
}
