using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sos.Application.Core.Abstractions.Cache;
using StackExchange.Redis;

namespace Sos.Infrastructure.Cache
{
    /// <summary>
    /// Represents the cache service.
    /// </summary>
    public sealed class CacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheService"/> class.
        /// </summary>
        /// <param name="distributedCache">The distributed cache.</param>
        /// <param name="connectionMultiplexer">The connection multiplexer.</param>
        public CacheService(
            IDistributedCache distributedCache,
            IConnectionMultiplexer connectionMultiplexer
        )
        {
            _distributedCache = distributedCache;
            _connectionMultiplexer = connectionMultiplexer;
        }

        // < inheritdoc />
        public async Task<string> GetAsync(string key)
        {
            var response = await _distributedCache.GetStringAsync(key);

            return (string.IsNullOrEmpty(response) ? null : response)!;
        }

        // < inheritdoc />
        public async Task RemoveAsync(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                return;
            }

            foreach (var key in GetKey(pattern))
            {
                await _distributedCache.RemoveAsync(key);
            }
        }

        private IEnumerable<string> GetKey(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                yield break;
            }

            foreach (var endpoint in _connectionMultiplexer.GetEndPoints())
            {
                var server = _connectionMultiplexer.GetServer(endpoint);

                foreach (var key in server.Keys(pattern: $"*{pattern}*"))
                {
                    yield return key.ToString();
                }
            }
        }

        // < inheritdoc />
        public async Task SetAsync(string key, object value, TimeSpan? expiredAt)
        {
            if (value == null)
            {
                return;
            }

            string response = JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            });

            await _distributedCache.SetStringAsync(key, response, new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = expiredAt
            });
        }
    }
}
