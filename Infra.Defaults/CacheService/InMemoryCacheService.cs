using Default.Application.Interfaces.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Defaults.CacheService
{
    public class InMemoryCacheService : ISimpleCacheService
    {
        private readonly ObjectCache _cache = MemoryCache.Default;
        public InMemoryCacheService()
        {
        }

        public async Task<TItem> GetOrCreate<TItem>(string key, Func<Task<TItem>> createItem, int expires = 86400)
        {
            var result = _cache.Get(key);

            await Task.Run(async () =>
            {
                if (result == null)
                {
                    //setting up cache options
                    var cacheItemPolicy = new CacheItemPolicy
                    {
                        AbsoluteExpiration = DateTimeOffset.Now.AddDays(1)
                    };
                    //setting cache entries
                    result = await createItem();
                    _cache.Add(key, result, cacheItemPolicy);
                }
            });

           

            return (TItem)result;
        }

        public TItem Get<TItem>(string key)
        {
            var cacheEntry = _cache.Get(key);
            return (TItem)cacheEntry;
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }

}
