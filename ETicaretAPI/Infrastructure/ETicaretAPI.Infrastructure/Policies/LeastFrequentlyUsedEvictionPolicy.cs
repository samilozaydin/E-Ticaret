using ETicaretAPI.Application.Abstractions.Policies;
using ETicaretAPI.Application.DTOs.Caches;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Policies
{
    public class LeastFrequentlyUsedEvictionPolicy : IEvictionPolicy
    {
        private readonly ConcurrentDictionary<string, int> accessCounts = new ConcurrentDictionary<string, int>();
        public bool Execute(ConcurrentDictionary<string, CacheItem> cacheItems)
        {
            if(cacheItems.IsEmpty || accessCounts.IsEmpty ) { return false; }

            //var cacheItem = accessCounts.MinBy(x => x.Value);
            var key = accessCounts.OrderBy(x => x.Value).FirstOrDefault().Key;

            if (key is not null)
            {
                cacheItems.TryRemove(key, out _);
                return accessCounts.TryRemove(key, out _);
            }
            return false;
        }   

        public void OnItemAccessed(string key)
        {
            accessCounts.AddOrUpdate(key,1,(_ ,count) => count + 1);
            
        }

        public void OnItemAdded(string key)
        {
            accessCounts[key] = 0;
        }

        public void OnItemRemoved(string key)
        {
            accessCounts.TryRemove(key, out _);

        }
    }
}
