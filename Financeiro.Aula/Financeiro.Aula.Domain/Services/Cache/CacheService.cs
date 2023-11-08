using Financeiro.Aula.Domain.Interfaces.Cache;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Financeiro.Aula.Domain.Services.Cache
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly DistributedCacheEntryOptions _options;

        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
            _options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2)
            };
        }

        public Task Set<T>(string key, T value)
        {
            return _cache.SetStringAsync(key, JsonConvert.SerializeObject(value), _options);
        }

        public async Task<T?> Get<T>(string key)
        {
            var value = await _cache.GetStringAsync(key);

            if (string.IsNullOrEmpty(value))
                return default;

            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}