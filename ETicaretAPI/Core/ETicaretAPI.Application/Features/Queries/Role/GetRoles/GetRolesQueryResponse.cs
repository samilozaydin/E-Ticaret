using ETicaretAPI.Application.DTOs.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.Role.GetRoles
{
    public class GetRolesQueryResponse
    {
        public List<RoleDTO> Roles { get; set; }
        public int TotalRoleCount { get; set; }
    }
}
