using System;
using System.Configuration;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using NLog;
using SFA.DAS.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.Configuration.FileStorage;
//using SFA.DAS.PSRService.Application.Infrastructure.Configuration;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Application.ReportHandlers;
using   SFA.DAS.PSRService.Application.Mapping;
using SFA.DAS.PSRService.Data;
using SFA.DAS.PSRService.Domain.Configuration;
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

        public Startup(IConfiguration config)
        {
            Configuration = ConfigurationService.GetConfig(config["Environment"], config["ConnectionStrings:Storage"], Version, ServiceName).Result;
        }

        public IWebConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {


            services.AddAndConfigureAuthentication(Configuration);
            services.AddMvc().AddControllersAsServices().AddSessionStateTempDataProvider();
            services.AddSession();

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
                config.For<IReportRepository>().Use<ReportRepository>().Ctor<string>().Is(Configuration.SqlConnectionString);
                
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
                        template: "{controller=Home}/{action=Index}/{id?}");
                });
        }

        private static PSRSServiceConfiguration GetConfigurationObject()
        {
            var environment = Environment.GetEnvironmentVariable("DASENV");
            if (string.IsNullOrEmpty(environment))
            {
                environment = CloudConfigurationManager.GetSetting("EnvironmentName");
            }

            var configurationRepository = GetConfigurationRepository();
            var configurationService = new SFA.DAS.Configuration.ConfigurationService(
                configurationRepository,
                new ConfigurationOptions(ServiceName, environment, "1.0"));

            var config = configurationService.Get<PSRSServiceConfiguration>();

            return config;
        }


        private static IConfigurationRepository GetConfigurationRepository()
        {
            IConfigurationRepository configurationRepository;
            if (bool.Parse(ConfigurationManager.AppSettings["LocalConfig"]))
            {
                configurationRepository = new FileStorageConfigurationRepository();
            }
            else
            {
                configurationRepository =
                    new AzureTableStorageConfigurationRepository(
                        CloudConfigurationManager.GetSetting("ConfigurationStorageConnectionString"));
            }

            return configurationRepository;
        }

    }

    public class Constants
    {
        private readonly string _baseUrl;
        public IdentityServerConfiguration Configuration { get; set; }
        public Constants(IdentityServerConfiguration configuration)
        {
            this.Configuration = configuration;
            _baseUrl = configuration.ClaimIdentifierConfiguration.ClaimsBaseUrl;
        }

        public string AuthorizeEndpoint() => $"{Configuration.BaseAddress}{Configuration.AuthorizeEndPoint}";
        public string LogoutEndpoint() => $"{Configuration.BaseAddress}{Configuration.LogoutEndpoint}";
        public string TokenEndpoint() => $"{Configuration.BaseAddress}{Configuration.TokenEndpoint}";
        public string UserInfoEndpoint() => $"{Configuration.BaseAddress}{Configuration.UserInfoEndpoint}";
        public string ChangePasswordLink() => Configuration.BaseAddress.Replace("/identity", "") + string.Format(Configuration.ChangePasswordLink, Configuration.ClientId);
        public string ChangeEmailLink() => Configuration.BaseAddress.Replace("/identity", "") + string.Format(Configuration.ChangeEmailLink, Configuration.ClientId);
        public string RegisterLink() => Configuration.BaseAddress.Replace("/identity", "") + string.Format(Configuration.RegisterLink, Configuration.ClientId);


        public string Id() => _baseUrl + Configuration.ClaimIdentifierConfiguration.Id;
        public string Email() => _baseUrl + Configuration.ClaimIdentifierConfiguration.Email;
        public string GivenName() => _baseUrl + Configuration.ClaimIdentifierConfiguration.GivenName;
        public string FamilyName() => _baseUrl + Configuration.ClaimIdentifierConfiguration.FamilyName;
        public string DisplayName() => _baseUrl + Configuration.ClaimIdentifierConfiguration.DisplayName;
        public string RequiresVerification() => _baseUrl + "requires_verification";
    }
}