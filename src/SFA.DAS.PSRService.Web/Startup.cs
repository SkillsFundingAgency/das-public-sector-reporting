using System;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Application.Mapping;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Data;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Configuration.Authorization;
using SFA.DAS.PSRService.Web.Extensions;
using SFA.DAS.PSRService.Web.Filters;
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
            services.AddHealthChecks();
            services.AddDataProtectionSettings(_hostingEnvironment, Configuration);
            services.AddMvc(opts =>
                {
                    opts.Filters.Add(new AuthorizeFilter(PolicyNames.HasEmployerAccount));
                    opts.Filters.AddService<GoogleAnalyticsFilter>();
                    opts.Filters.AddService<ZenDeskApiFilter>();
                })
                .AddControllersAsServices().AddSessionStateTempDataProvider();

            services.AddSession(config => config.IdleTimeout = TimeSpan.FromHours(1));
            services.AddAutoMapper(typeof(ReportMappingProfile), typeof(AuditRecordMappingProfile));

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
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseExceptionHandler("/Home/Error");
            }

            // Add Content Security Policy           
            app.UseCsp(options => options
                .DefaultSources(s =>
                {
                    s.Self()
                        .CustomSources(
                        "https://*.zdassets.com",
                        "https://*.zendesk.com",
                        "wss://*.zendesk.com",
                        "wss://*.zopim.com",
                        "https://*.rcrsv.io",
                        "https://assets.publishing.service.gov.uk"
                        );
                })
                .StyleSources(s =>
                {
                    s.Self()
                    .CustomSources("https://www.googletagmanager.com/",
                                    "https://www.tagmanager.google.com/",
                                    "https://tagmanager.google.com/",
                                    "https://fonts.googleapis.com/",
                                    "https://*.zdassets.com",
                                    "https://*.zendesk.com",
                                    "wss://*.zendesk.com",
                                    "wss://*.zopim.com",
                                    "https://*.rcrsv.io"
                                    );
                    s.UnsafeInline();
                }
                )
                .ScriptSources(s =>
                {
                    s.Self()
                        .CustomSources("https://az416426.vo.msecnd.net/scripts/a/ai.0.js",
                                    "*.google-analytics.com",
                                     "*.googleapis.com",
                                    "*.googletagmanager.com/",
                                    "https://www.tagmanager.google.com/",
                                    "https://*.zdassets.com",
                                    "https://*.zendesk.com",
                                    "wss://*.zendesk.com",
                                    "wss://*.zopim.com",
                                    "https://*.rcrsv.io");
                    //Google tag manager uses inline scripts when administering tags. This is done on PREPROD only
                    if (env.IsEnvironment(EnvironmentNames.PREPROD))
                    {
                        s.UnsafeInline();
                        s.UnsafeEval();
                    }
                })
                .FontSources(s =>
                    s.Self()
                    .CustomSources("data:",
                                    "https://fonts.googleapis.com/",
                                    "https://assets-ukdoe.rcrsv.io/")
                )
                .ConnectSources(s =>
                    s.Self()
                    .CustomSources(
                        "https://*.zendesk.com",
                        "https://*.zdassets.com",
                        "https://dc.services.visualstudio.com",
                        "wss://*.zendesk.com",
                        "wss://*.zopim.com",
                        "https://*.rcrsv.io")
                )
                .ImageSources(s =>
                    {
                        s.Self()
                            .CustomSources(
                                "*.google-analytics.com",
                                "https://ssl.gstatic.com",
                                "https://www.gstatic.com/",
                                "https://*.zopim.io",
                                "https://*.zdassets.com",
                                "https://*.zendesk.com",
                                "wss://*.zendesk.com",
                                "wss://*.zopim.com",
                                "data:",
                                "https://assets.publishing.service.gov.uk"
                                );
                    }
                )
                .ReportUris(r => r.Uris("/ContentPolicyReport/Report")));


            app.UseStaticFiles()
                .UseHttpsRedirection()
                .UseErrorLoggingMiddleware()
                .UseSession()
                .UseAuthentication()
                .UseHealthChecks("/ping")
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
