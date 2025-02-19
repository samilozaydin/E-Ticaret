using ETicaretAPI.Application.Abstractions.Cache.Redis;
using ETicaretAPI.Application.DTOs.Caches;
using ETicaretAPI.Infrastructure.Options;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services.Cache.Redis
{
    public class RedisCache : Cache, IRedisCache
    {
        private readonly RedisCacheOptions _opt;
        private readonly IDistributedCache _redis;
        private readonly IConnectionMultiplexer _redisConnection;

        public RedisCache(IDistributedCache redis, RedisCacheOptions opt, IConnectionMultiplexer redisConnection)
        {
            _redis = redis;
            _opt = opt;
            _redisConnection = redisConnection;
        }

        public bool Contains(string key)
        {
            string cacheItem = _redis.GetString(key);

            if(string.IsNullOrEmpty(cacheItem)) {
                return false;
            }

            return true;
        }

        public CacheItem Get(string key)
        {
            string cacheItem = _redis.GetString(key);

            if (string.IsNullOrEmpty(cacheItem))
            {
                return default;
            }
            return JsonSerializer.Deserialize<CacheItem>(cacheItem); 
        }

        public ReadOnlyDictionary<string, CacheItem> GetItems()
        {
            var server = _redisConnection.GetServer(_redisConnection.GetEndPoints().First());

            var keys = server.Keys();
            var items = new Dictionary<string,CacheItem>();

            foreach (var key in keys)
            {
                var value = Get(key);
                items.Add(key,value);
            }

            return new ReadOnlyDictionary<string,CacheItem>(items);
        }

        public IEnumerable<string> GetKeys()
        {

            var server = _redisConnection.GetServer(_redisConnection.GetEndPoints().First());
            return server.Keys().Select(key => key.ToString());
        }

        public T GetValue<T>(string key)
        {
            var cacheItem = Get(key);
            return cacheItem is null ? default : cacheItem.GetValue<T>();
        }

        public IEnumerable<CacheItem> GetValues()
        {
            var keys = GetKeys();
            var values = new List<CacheItem>();

            foreach (var key in keys)
            {
                var value = Get(key);
                values.Add(value);
            }

            return values;
        }

        public bool IsExpired(string key)
        {
            var cacheItem = Get(key) ?? throw new ArgumentException("CacheItem not found");

            return cacheItem is not null &&
                   cacheItem.ExpirationTime < DateTime.UtcNow;
        }

        public bool Remove(string key)
        {
            _redis.Remove(key);
            return true;
        }

        public bool RemoveIfExpired(string key) => IsExpired(key) && Remove(key);

        public bool Set(string key, object value, TimeSpan? expirationTime = null)
        {
            var jsonValue = value is not string
                            ? JsonSerializer.Serialize(value, new JsonSerializerOptions() { WriteIndented = true})
                            : value.ToString();
            var expiryDate = GetExpiryDate(expirationTime);

            var cacheItem = new CacheItem(key, jsonValue, expiryDate);
            var jsonCacheItem = JsonSerializer.Serialize(cacheItem, new JsonSerializerOptions() { WriteIndented = true });

            _redis.SetString(key, jsonCacheItem, new DistributedCacheEntryOptions() {AbsoluteExpiration = expiryDate});

            return true;
        }

        public bool TryGet(string key, out CacheItem item)
        {
            item = Get(key);
            return item != null;
        }
        private DateTime? GetExpiryDate(TimeSpan? expiry)
        {
            TimeSpan? expiryDate = expiry ?? _opt.DefaultExpiry;

            return expiryDate.HasValue
                ? DateTime.UtcNow.Add(expiryDate.Value)
                : default;
        }
    }
}
