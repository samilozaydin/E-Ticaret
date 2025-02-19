using ETicaretAPI.Application.Abstractions.Policies;
using ETicaretAPI.Application.DTOs.Caches;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Policies
{
    public class FirstInFirstOutEvictionPolicy : IEvictionPolicy
    {
        public bool Execute(ConcurrentDictionary<string, CacheItem> cacheItems)
        {
            var firstItem = cacheItems.First();
            
            return cacheItems.TryRemove(firstItem);

        }
        public void OnItemAccessed(string key)
        {
            throw new NotImplementedException();
        }

        public void OnItemAdded(string key)
        {
            throw new NotImplementedException();
        }

        public void OnItemRemoved(string key)
        {
            throw new NotImplementedException();
        }
    }
}
