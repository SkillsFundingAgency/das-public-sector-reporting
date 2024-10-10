using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.PSRService.Web.Configuration;

namespace SFA.DAS.PSRService.Web.StartupConfiguration;

public static class ConfigurationServiceRegistrations
{
    public static IServiceCollection AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<WebConfiguration>(configuration);
        
        services.AddOptions();
        
        return services;
    }
}