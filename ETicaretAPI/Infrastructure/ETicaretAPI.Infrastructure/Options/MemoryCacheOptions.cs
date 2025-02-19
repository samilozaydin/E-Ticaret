using ETicaretAPI.Application.Abstractions.Policies;
using ETicaretAPI.Infrastructure.Enums;
using ETicaretAPI.Infrastructure.Policies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Options
{
    public class MemoryCacheOptions
    {
        internal IEvictionPolicy EvictionPolicy { get; set; }
        //public TimeProvider TimeProvider { get; set; } = TimeProvider.System
        public int? Capacity { get; set; }
        public TimeSpan? DefaultExpiry { get; set; }
        public StringComparer KeyComparer { get; set; } = StringComparer.OrdinalIgnoreCase;
    }

    public class MemoryCacheOptionsBuilder
    {
        private readonly MemoryCacheOptions opt = new MemoryCacheOptions();

       /* public MemoryCacheOptionsBuilder WithTimeProvider(TimeProvider provider)
        {
            opt.TimeProvider = timeProvider;
            return this;
        }*/

        public MemoryCacheOptionsBuilder SetDefaultExpiry(TimeSpan? expiry)
        {
            opt.DefaultExpiry = expiry ?? default;
            return this;
        }
        public MemoryCacheOptionsBuilder SetCapacity(int? capacity, CacheEvictionPolicies policy)
        {
            opt.Capacity = capacity ?? default;

            opt.EvictionPolicy = policy switch
            {
                CacheEvictionPolicies.FirstInFirstOut => new FirstInFirstOutEvictionPolicy(),
                CacheEvictionPolicies.LastInFirstOut => new LastInFirstOutEvictionPolicy(),
                CacheEvictionPolicies.LeastFrequentlyUsed => new LeastFrequentlyUsedEvictionPolicy(),
                _ or CacheEvictionPolicies.None => new NullEvictionPolicy()
            };

            return this;
        }
        public MemoryCacheOptions Build()
        {
            return opt;
        }
    }
}
