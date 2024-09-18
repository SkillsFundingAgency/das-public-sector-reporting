using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.IdentityModel.Logging;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.Employer.Shared.UI;
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

namespace SFA.DAS.PSRService.Web;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _hostingEnvironment;
    private readonly IWebConfiguration _webConfiguration;

    public Startup(IConfiguration configuration, IHostEnvironment env)
    {
        _hostingEnvironment = env;
        _configuration = configuration.BuildDasConfiguration();
        _webConfiguration = _configuration.GetSection(nameof(WebConfiguration)).Get<WebConfiguration>();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IEmployerAccountService, EmployerAccountService>();
        services.AddTransient<IAccountApiClient, AccountApiClient>();
        services.AddTransient<IAccountApiConfiguration, AccountApiConfiguration>();
        services.AddSingleton<IAccountApiConfiguration>(_webConfiguration.AccountsApi);

        services.AddLogging(builder =>
        {
            builder.AddFilter<ApplicationInsightsLoggerProvider>(string.Empty, LogLevel.Information);
            builder.AddFilter<ApplicationInsightsLoggerProvider>("Microsoft", LogLevel.Information);
        });

        services.AddSingleton(_webConfiguration.OuterApiConfiguration);
        services.AddHttpClient<IOuterApiClient, OuterApiClient>();
        services.AddTransient<IEmployerUserAccountsService, EmployerUserAccountsService>();

        services.AddAndConfigureAuthentication(_webConfiguration, _configuration);
        if (_webConfiguration.UseGovSignIn)
        {
            services.AddMaMenuConfiguration("SignOut", _configuration["ResourceEnvironmentName"]);
        }
        else
        {
            services.AddMaMenuConfiguration("SignOut", _webConfiguration.Identity.ClientId, _configuration["ResourceEnvironmentName"]);
        }

        services.AddAuthorizationService();
        services.AddHealthChecks();
        services.AddDataProtectionSettings(_hostingEnvironment, _webConfiguration);
        services.AddMvc(opts =>
            {
                opts.EnableEndpointRouting = false;
                opts.Filters.Add(new AuthorizeFilter());
                opts.Filters.AddService<GoogleAnalyticsFilter>();
                opts.Filters.AddService<ZenDeskApiFilter>();
            })
            .AddControllersAsServices()
            .AddSessionStateTempDataProvider()
            .SetDefaultNavigationSection(NavigationSection.ApprenticesHome);

        services.AddSession(config => config.IdleTimeout = TimeSpan.FromHours(1));
        services.AddAutoMapper(typeof(ReportMappingProfile), typeof(AuditRecordMappingProfile));
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<SubmitReportHandler>());
        
        services.AddSingleton(_webConfiguration);
        services.AddDatabaseRegistration(_webConfiguration.SqlConnectionString);
        services.AddScoped<IReportRepository, SqlReportRepository>();
        
        var physicalProvider = _hostingEnvironment.ContentRootFileProvider;
        services.AddSingleton(physicalProvider);
        
        services.AddApplicationInsightsTelemetry();
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
}