using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Enums
{
    public enum CacheEvictionPolicies
    {
        None = 0,
        LastInFirstOut = 1,
        FirstInFirstOut = 2,
        LeastFrequentlyUsed = 3 
    }
}
