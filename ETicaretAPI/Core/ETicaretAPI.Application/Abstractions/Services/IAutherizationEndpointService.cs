using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions.Services
{
    public interface IAutherizationEndpointService
    {
        public Task AssignRoleEndpointsAsync(string[] roles,string menu, string code, Type type);
        public Task<List<string>> GetRolesToEndpoint(string code,string menu);
    }
}
