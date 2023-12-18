using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Default.Application.Interfaces.Cache
{
    public interface ISimpleCacheService
    {
        Task<TItem> GetOrCreate<TItem>(string key, Func<Task<TItem>> createItem, int expires = 86400);
        TItem Get<TItem>(string key);
        void Remove(string key);
    }
}
