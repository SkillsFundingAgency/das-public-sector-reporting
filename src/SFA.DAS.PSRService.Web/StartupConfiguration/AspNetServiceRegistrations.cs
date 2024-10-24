using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Employer.Shared.UI;
using SFA.DAS.PSRService.Web.Filters;

namespace SFA.DAS.PSRService.Web.StartupConfiguration;

public static class AspNetServiceRegistrations
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
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

        services.AddScoped<GoogleAnalyticsFilter>();
        services.AddScoped<ZenDeskApiFilter>();

        return services;
    }
}