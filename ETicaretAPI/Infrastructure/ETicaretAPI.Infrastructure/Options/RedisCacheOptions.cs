using ETicaretAPI.Infrastructure.Enums;
using ETicaretAPI.Infrastructure.Policies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Options
{
    public class RedisCacheOptions
    {
        public TimeSpan? DefaultExpiry { get; set; }
    }
    public class RedisCacheOptionsBuilder
    {
        private readonly RedisCacheOptions opt = new RedisCacheOptions();

        public RedisCacheOptionsBuilder SetDefaultExpiry(TimeSpan? expiry)
        {
            opt.DefaultExpiry = expiry ?? default;
            return this;
        }

        public RedisCacheOptions Build()
        {
            return opt;
        }
    }
}
