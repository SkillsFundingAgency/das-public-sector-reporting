using System.IO;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.Employer.Shared.UI;
using SFA.DAS.Forecasting.Web.Extensions;
using SFA.DAS.PSRService.Application.EmployerUserAccounts;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Application.Mapping;
using SFA.DAS.PSRService.Application.OuterApi;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Data;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Extensions;
using SFA.DAS.PSRService.Web.Filters;
using SFA.DAS.PSRService.Web.Services;
using SFA.DAS.PSRService.Web.StartupConfiguration;
using StructureMap;

namespace SFA.DAS.PSRService.Web
{
    public class Startup
    {
        private readonly IConfigurationRoot _config;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private IWebConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _hostingEnvironment = env;
            var config = new ConfigurationBuilder()
                .AddConfiguration(configuration)
                .SetBasePath(Directory.GetCurrentDirectory());

#if DEBUG
            if (!configuration.IsDev())
            {
                config.AddJsonFile("appsettings.json", false)
                    .AddJsonFile("appsettings.Development.json", true);
            }
#endif

            config.AddEnvironmentVariables();
            config.AddAzureTableStorage(options =>
                {
                    options.ConfigurationKeys = configuration["ConfigNames"].Split(",");
                    options.StorageConnectionString = configuration["ConfigurationStorageConnectionString"];
                    options.EnvironmentName = configuration["EnvironmentName"];
                    options.PreFixConfigurationKeys = false;
                }
            );

            _config = config.Build();
            Configuration = _config.GetSection(nameof(WebConfiguration)).Get<WebConfiguration>();
        }



        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IEmployerAccountService, EmployerAccountService>();
            services.AddTransient<IAccountApiClient, AccountApiClient>();
            services.AddTransient<IAccountApiConfiguration, AccountApiConfiguration>();
            services.AddSingleton<IAccountApiConfiguration>(Configuration.AccountsApi);

            services.AddSingleton(Configuration.OuterApiConfiguration);
            services.AddHttpClient<IOuterApiClient, OuterApiClient>();
            services.AddTransient<IEmployerUserAccountsService, EmployerUserAccountsService>();

            services.AddAndConfigureAuthentication(Configuration, _config);
            if (Configuration.UseGovSignIn)
            {
                services.AddMaMenuConfiguration("SignOut", _config["ResourceEnvironmentName"]);   
            }
            else
            {
                services.AddMaMenuConfiguration("SignOut", Configuration.Identity.ClientId, _config["ResourceEnvironmentName"]);    
            }
            services.AddAuthorizationService();
            services.AddHealthChecks();
            services.AddDataProtectionSettings(_hostingEnvironment, Configuration);
            services.AddMvc(opts =>
                {
                    opts.EnableEndpointRouting = false;
                    opts.Filters.Add(new AuthorizeFilter());
                    opts.Filters.AddService<GoogleAnalyticsFilter>();
                    opts.Filters.AddService<ZenDeskApiFilter>();
                })
                .AddControllersAsServices().AddSessionStateTempDataProvider()
                .SetDefaultNavigationSection(NavigationSection.ApprenticesHome);

            services.AddSession(config => config.IdleTimeout = TimeSpan.FromHours(1));
            services.AddAutoMapper(typeof(ReportMappingProfile), typeof(AuditRecordMappingProfile));
            services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<SubmitReportHandler>());

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
                config.AddDatabaseRegistration(Configuration.SqlConnectionString);
                config.For<IReportRepository>().Use<SqlReportRepository>();
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
              
                config.For<IMediator>().Use<Mediator>();
            });

            return container.GetInstance<IServiceProvider>();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                IdentityModelEventSource.ShowPII = true;
            }
            else
            {
                app.UseHsts();
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                Secure = CookieSecurePolicy.Always
            });

            app.UseStaticFiles()
                .UseHttpsRedirection()
                .UseErrorLoggingMiddleware()
                .UseSession()
                .UseAuthentication()
                .UseHealthChecks("/ping");

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "accounts/{hashedEmployerAccountId}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                        name: "Service-Controller",
                        pattern: "Service/{action}",
                        defaults: new { controller = "Service" });
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
