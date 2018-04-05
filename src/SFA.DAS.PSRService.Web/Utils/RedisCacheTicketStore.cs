using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using SFA.DAS.PSRService.Web.Configuration;

namespace SFA.DAS.PSRService.Web.Utils
{
    public class RedisCacheTicketStore : ITicketStore
    {
        private readonly IDistributedCache _cache;
        private IWebConfiguration _webConfiguration;
        private const string KeyPrefix = "AuthSessionStore-";
        //TODO : Need to Configure this
        public static int RedisPort = 6379; // override default so that do not interfere with anyone else's server

        public RedisCacheTicketStore(string redisConnectionString)
        {
           
            _cache = new RedisCache(new RedisCacheOptions
            {
                //TODO : Need to Configure this
                Configuration = string.IsNullOrEmpty(redisConnectionString)? ("localhost:" + RedisPort) : redisConnectionString,
                InstanceName = "PSRS.Master."
            });
        }

        public Task<string> StoreAsync(AuthenticationTicket ticket)
        {
            var guid = Guid.NewGuid();
            var key = KeyPrefix + guid;
            RenewAsync(key, ticket);
            return Task.FromResult(key);
        }

        public Task RenewAsync(string key, AuthenticationTicket ticket)
        {
            var options = new DistributedCacheEntryOptions();
            var expiresUtc = ticket.Properties.ExpiresUtc;
            if (expiresUtc.HasValue)
            {
                options.SetAbsoluteExpiration(expiresUtc.Value);
            }

            //TODO : Need to Configure this
            options.SetSlidingExpiration(TimeSpan.FromMinutes(2));

            _cache.Set(key, SerializeToBytes(ticket), new DistributedCacheEntryOptions());

            return Task.FromResult(0);
        }

        public Task<AuthenticationTicket> RetrieveAsync(string key)
        {
            var ticket = DeserializeFromBytes(_cache.Get(key));
            return Task.FromResult(ticket);
        }

        public Task RemoveAsync(string key)
        {
            _cache.Remove(key);
            return Task.FromResult(0);
        }

        private static byte[] SerializeToBytes(AuthenticationTicket source)
        {
            return TicketSerializer.Default.Serialize(source);
        }

        private static AuthenticationTicket DeserializeFromBytes(byte[] source)
        {
            return source == null ? null : TicketSerializer.Default.Deserialize(source);
        }
    }
}
