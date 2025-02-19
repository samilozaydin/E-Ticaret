using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.DTOs.Caches
{
    public class CacheItem
    {
        public CacheItem() { }

        public CacheItem(string key, object value)
        {
            Key = key;
            Value = value;
        }

        public CacheItem(string key, object value, DateTime? expirationTime) : this(key, value)
        {
            ExpirationTime = expirationTime;
        }

        public string Key { get; set; }
        public object Value { get; set; }
        public DateTime? ExpirationTime { get; set; }

        public T GetValue<T>() => (T)Value;
    }
}
