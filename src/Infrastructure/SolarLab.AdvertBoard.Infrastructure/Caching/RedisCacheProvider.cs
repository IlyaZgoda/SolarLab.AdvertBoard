using Microsoft.Extensions.Caching.Distributed;
using SolarLab.AdvertBoard.Application.Abstractions.Caching;
using System.Text.Json;

namespace SolarLab.AdvertBoard.Infrastructure.Caching
{
    public class RedisCacheProvider(IDistributedCache cache) : ICacheProvider
    {
        private readonly JsonSerializerOptions _options = new(JsonSerializerDefaults.Web);

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var data = await cache.GetStringAsync(key, cancellationToken);
            return data is null ? default : JsonSerializer.Deserialize<T>(data, _options);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            var json = JsonSerializer.Serialize(value, _options);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromHours(1)
            };
            await cache.SetStringAsync(key, json, options, cancellationToken);
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            await cache.RemoveAsync(key, cancellationToken);
        }
    }
}
