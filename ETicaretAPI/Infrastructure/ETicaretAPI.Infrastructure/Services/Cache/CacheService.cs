using ETicaretAPI.Application.Abstractions.Cache;
using ETicaretAPI.Application.DTOs.Caches;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services.Cache
{
    public class CacheService : ICacheService
    {
        readonly ICache _cache;

        public CacheService(ICache cache)
        {
            _cache = cache;
            
        }

        public bool Contains(string key) => _cache.Contains(key);

        public CacheItem Get(string key) => _cache.Get(key);

        public ReadOnlyDictionary<string, CacheItem> GetItems() => _cache.GetItems();
        public IEnumerable<string> GetKeys() => _cache.GetKeys();

        public T GetValue<T>(string key) => _cache.GetValue<T>(key);

        public IEnumerable<CacheItem> GetValues() => _cache.GetValues();

        public bool IsExpired(string key) => _cache.IsExpired(key);

        public bool Remove(string key) => _cache.Remove(key);

        public bool RemoveIfExpired(string key) => _cache.RemoveIfExpired(key);
        public bool Set(string key, object value, TimeSpan? expirationTime = null) => _cache.Set(key, value, expirationTime);

        public bool TryGet(string key, out CacheItem item) => _cache.TryGet(key, out item);
    }
}
