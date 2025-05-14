using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using SFA.DAS.Employer.Shared.UI;
using SFA.DAS.PSRService.Web.Filters;

namespace SFA.DAS.PSRService.Web.StartupConfiguration;

public static class AspNetServiceRegistrations
{
    public static IServiceCollection AddWebServices(this IServiceCollection services,
        IConfiguration webConfiguration)
    {
        services.AddMvc(opts =>
            {
                opts.EnableEndpointRouting = false;
                opts.Filters.AddService<GoogleAnalyticsFilter>();
                opts.Filters.AddService<ZenDeskApiFilter>();
                opts.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            })
            .AddControllersAsServices()
            .AddSessionStateTempDataProvider()
            .SetDefaultNavigationSection(NavigationSection.ApprenticesHome);

        services.AddAntiforgery(options =>
        {
            options.Cookie.Name = "psr-x-csrf";
            options.FormFieldName = "_csrfToken";
            options.HeaderName = "X-XSRF-TOKEN";
        });

        services.AddScoped<GoogleAnalyticsFilter>();
        services.AddScoped<ZenDeskApiFilter>();
        
        

        return services;
    }
}