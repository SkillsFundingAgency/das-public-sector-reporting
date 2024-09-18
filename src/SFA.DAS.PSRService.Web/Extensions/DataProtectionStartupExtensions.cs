using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SFA.DAS.PSRService.Web.Configuration;

namespace SFA.DAS.PSRService.Web.Extensions;

public static class DataProtectionStartupExtensions
{
    public static IServiceCollection AddDataProtectionSettings(this IServiceCollection services, IHostEnvironment environment, IWebConfiguration config)
    {
        if (environment.IsDevelopment() || config == null) return services;

        var redisConnectionString = config.SessionStore.Connectionstring;
        var dataProtectionKeysDatabase = config.DataProtectionKeysDatabase;

        var redis = StackExchange.Redis.ConnectionMultiplexer
            .Connect($"{redisConnectionString},{dataProtectionKeysDatabase}");

        services.AddDataProtection()
            .SetApplicationName("das-employer")
            .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");

        return services;
    }
}