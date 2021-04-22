using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.PSRService.Web.Configuration;
using StackExchange.Redis;

namespace SFA.DAS.PSRService.Web.Extensions
{
    public static class DataProtectionStartupExtensions
    {
        public static IServiceCollection AddDataProtectionSettings(this IServiceCollection services, IHostingEnvironment environment, IWebConfiguration config)
        {
            if (environment.IsDevelopment() || config == null) return services;

            var redisConnectionString = config.SessionStore.Connectionstring;
            var dataProtectionKeysDatabase = config.DataProtectionKeysDatabase;

            var redis = StackExchange.Redis.ConnectionMultiplexer
                .Connect($"{redisConnectionString},{dataProtectionKeysDatabase}");

            services.AddDataProtection()
                .SetApplicationName("das-public-sector-reporting-web")
                .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");

            return services;
        }

    }
}
