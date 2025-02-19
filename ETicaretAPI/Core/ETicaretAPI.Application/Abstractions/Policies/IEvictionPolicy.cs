using ETicaretAPI.Application.DTOs.Caches;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions.Policies
{
    public interface IEvictionPolicy
    {
        bool Execute(ConcurrentDictionary<string,CacheItem> cacheItems);

        void OnItemAdded(string key);
        void OnItemRemoved(string key);
        void OnItemAccessed(string key);
    }
}
