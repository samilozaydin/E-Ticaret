using ETicaretAPI.Application.DTOs.Caches;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions.Cache
{
    public interface ICache
    {
        IEnumerable<string> GetKeys();
        IEnumerable<CacheItem> GetValues();
        System.Collections.ObjectModel.ReadOnlyDictionary<string, CacheItem> GetItems();
        bool Set(string key, object value, TimeSpan? expirationTime = null);
        CacheItem Get(string key);
        bool Contains(string key);  
        T GetValue<T>(string key);
        bool TryGet(string key, out CacheItem item);
        bool IsExpired(string key);
        bool Remove(string key);
        bool RemoveIfExpired(string key);

    }
}
