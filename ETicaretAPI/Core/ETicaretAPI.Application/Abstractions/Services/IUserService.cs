using ETicaretAPI.Application.DTOs.User;
using ETicaretAPI.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<CreateUserResponse> CreateAsync(CreateUser model);
        Task UpdateRefreshToken(string refreshToken, AppUser user, DateTime expire, int AddOnExpireTime);
        Task UpdatePassword(string userId, string resetToken, string password);
        Task<List<ListUser>> GetAllUsers(int page,int size);
        int TotalUsersCount { get; }
        Task AssignRoleToUser(string id, string[] roles);
        Task<List<string>> GetRolesToUser(string userIdOrName);
        Task<bool> HasRolePermissonToEndpointAsync(string name, string code);

    }
}
