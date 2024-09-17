using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;

namespace SFA.DAS.PSRService.Web.Utils;

public class RedisCacheTicketStore : ITicketStore
{
    private readonly RedisCache _cache;
    private const string KeyPrefix = "AuthSessionStore-";
    private const int RedisPort = 6379; // override default so that do not interfere with anyone else's server

    public RedisCacheTicketStore(string redisConnectionString)
    {
        _cache = new RedisCache(new RedisCacheOptions
        {
            Configuration = string.IsNullOrEmpty(redisConnectionString) ? "localhost:" + RedisPort : redisConnectionString,
            InstanceName = "PSRS.Master."
        });
    }

    public async Task<string> StoreAsync(AuthenticationTicket ticket)
    {
        var guid = Guid.NewGuid();
        var key = KeyPrefix + guid;

        await RenewAsync(key, ticket);

        return key;
    }

    public async Task RenewAsync(string key, AuthenticationTicket ticket)
    {
        var options = new DistributedCacheEntryOptions();
        var expiresUtc = ticket.Properties.ExpiresUtc;
        if (expiresUtc.HasValue)
        {
            options.SetAbsoluteExpiration(expiresUtc.Value);
        }

        options.SetSlidingExpiration(TimeSpan.FromMinutes(2));

        await _cache.SetAsync(key, SerializeToBytes(ticket), new DistributedCacheEntryOptions());
    }

    public Task<AuthenticationTicket> RetrieveAsync(string key)
    {
        var ticket = DeserializeFromBytes(_cache.Get(key));
        return Task.FromResult(ticket);
    }

    public async Task RemoveAsync(string key)
    {
        await _cache.RemoveAsync(key);
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