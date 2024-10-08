using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.Role;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Services
{
    public class RoleService : IRoleService
    {
        readonly RoleManager<AppRole> _roleManager;

        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<bool> CreateRole(string name)
        {
           IdentityResult result=  await _roleManager.CreateAsync(new() { Name= name,Id= Guid.NewGuid().ToString()});
           return result.Succeeded;
        }

        public async Task<bool> DeleteRole(string id)
        {
            AppRole role = await _roleManager.FindByIdAsync(id);
            IdentityResult result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;

        }

        public async Task<List<RoleDTO>> GetAllRoles(int page,int size)
        {
            var query = _roleManager.Roles;

            if (page != -1 && size != -1)
                query = query.Skip(page * size).Take(size);

            return await query.Select(r => new RoleDTO { Id= r.Id, Name =r.Name}).ToListAsync();

        }

        public async Task<(string id, string name)> GetRoleById(string id)
        {
            var role = await _roleManager.GetRoleIdAsync(new AppRole() { Id = id });

            return (id,role);
        }

        public async Task<bool> UpdateRole(string id,string name)
        {
            AppRole role = await _roleManager.FindByIdAsync(id);
            role.Name = name;
            IdentityResult result = await _roleManager.UpdateAsync(role);
            return result.Succeeded;
        }
    }
}
