using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions.Policies
{
    public interface ILeastFrequentlyUsedEvictionPolicy : IEvictionPolicy
    {
        void OnItemAdded(string key);
        void OnItemRemoved(string key);
        void OnItemAccessed(string key);
    }
}
