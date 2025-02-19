using MyCache=  ETicaretAPI.Application.Abstractions.Cache.MemoryCache;
using OPT = ETicaretAPI.Infrastructure.Options;
using ETicaretAPI.Application.DTOs.Caches;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaretAPI.Infrastructure.Options;
using System.Collections.Immutable;

namespace ETicaretAPI.Infrastructure.Services.Cache.MemoryCache
{
    public class MemoryCache : Cache, MyCache.IMemoryCache
    {
        private readonly OPT.MemoryCacheOptions opt;
        private readonly ConcurrentDictionary<string, CacheItem> cacheItems;

        public MemoryCache(OPT.MemoryCacheOptions opt)
        {
            ArgumentNullException.ThrowIfNull(opt);
            cacheItems = new(opt.KeyComparer);
            this.opt = opt;
        }

        public bool Contains(string key) => cacheItems.ContainsKey(key);

        public CacheItem Get(string key)
        {
            if(cacheItems.TryGetValue(key, out var cacheItem))
            {
                opt.EvictionPolicy.OnItemAccessed(key);
                return cacheItem;
            }
            return default;
   
        }

        public ReadOnlyDictionary<string, CacheItem> GetItems() => new ReadOnlyDictionary<string, CacheItem>(cacheItems);

        public IEnumerable<string> GetKeys() => cacheItems.Keys;

        public T GetValue<T>(string key)
        {
            var cacheItem = Get(key);
            return cacheItem is null ? default : cacheItem.GetValue<T>();
        }

        public IEnumerable<CacheItem> GetValues() => cacheItems.Values;

        public bool IsExpired(string key)
        {
            var cacheItem = Get(key) ?? throw new ArgumentException("CacheItem not found");

            return cacheItem is not null &&
                   cacheItem.ExpirationTime < DateTime.UtcNow;
        }

        public bool Remove(string key)
        {
            opt.EvictionPolicy.OnItemRemoved(key);
            return cacheItems.TryRemove(key, out _);
        }

        public bool RemoveIfExpired(string key) => IsExpired(key) && Remove(key);

        public bool Set(string key, object value, TimeSpan? expirationTime = null)
        {
            ArgumentNullException.ThrowIfNull(nameof(key));
            EnsureCapacity();

            var expiryDate = GetExpiryDate(expirationTime);
            cacheItems[key] = new CacheItem(key, value, expiryDate);
            opt.EvictionPolicy.OnItemAdded(key);
            return true;
        }

        public bool TryGet(string key, out CacheItem item)
        {
            item = Get(key);
            return item != null;

        }
        private void EnsureCapacity()
        {
            if (!opt.Capacity.HasValue)
                return;
            
            if(cacheItems.Count >= opt.Capacity.Value)
            {
                opt.EvictionPolicy.Execute(cacheItems);
            }
        }
        private DateTime? GetExpiryDate(TimeSpan? expiry)
        {
            TimeSpan? expiryDate = expiry ?? opt.DefaultExpiry;

            return expiryDate.HasValue
                ? DateTime.UtcNow.Add(expiryDate.Value)
                : default;
        }
    }
}
