using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.User;
using ETicaretAPI.Application.Exceptions;
using ETicaretAPI.Application.Features.Commands.AppUser.CreateUser;
using ETicaretAPI.Application.Helpers;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ET = ETicaretAPI.Domain.Entities;

namespace ETicaretAPI.Persistence.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<ET.Identity.AppUser> _userManager;
        readonly IEndpointReadRepository _endpointReadRepository;
        public int TotalUsersCount =>  _userManager.Users.Count();

        public UserService(UserManager<ET.Identity.AppUser> userManager, IEndpointReadRepository endpointReadRepository)
        {
            _userManager = userManager;
            _endpointReadRepository = endpointReadRepository;
        }

        public async Task<CreateUserResponse> CreateAsync(CreateUser model)
        {
            IdentityResult result = await _userManager.CreateAsync(new ET.Identity.AppUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Email = model.Email,    
                NameSurname = model.NameSurname
            }, model.Password);

            CreateUserResponse response = new CreateUserResponse { Succeeded = result.Succeeded };
            if (result.Succeeded)
            {
                response.Message = "User is created";
            }
            else
            {
                foreach (var error in result.Errors)
                    response.Message += $"{error.Code} - {error.Description}\n";
            }

            return response;
        }

        public async Task UpdateRefreshToken(string refreshToken, AppUser user, DateTime expire, int AddOnExpireTime)
        {
            if (user != null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenEndDate = expire.AddSeconds(AddOnExpireTime);
                await _userManager.UpdateAsync(user);
            }
            else
                throw new NotFoundUserException();

        }
        public async Task UpdatePassword(string userId, string resetToken, string password)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            if(user != null)
            {
                resetToken = resetToken.UrlDecode();

                IdentityResult result = await _userManager.ResetPasswordAsync(user,resetToken,password);
                if (result.Succeeded)
                {
                    await _userManager.UpdateSecurityStampAsync(user);
                }
                else
                {
                    throw new PasswordChangeFailedException();
                }
            }

        }

        public async Task<List<ListUser>> GetAllUsers(int page,int size)
        {
            var users = await _userManager.Users
                .Skip(page*size)
                .Take(size)
                .ToListAsync();
            return  users.Select(u => new ListUser()
            {
                Id = u.Id,
                Email = u.Email,
                NameSurname = u.NameSurname,
                TwoFactorEnabled = u.TwoFactorEnabled,
                Username = u.UserName
            }).ToList();
        }

        public async Task AssignRoleToUser(string id, string[] roles)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

            if(user != null )
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                if (userRoles != null) 
                    await _userManager.RemoveFromRolesAsync(user,userRoles);
                await _userManager.AddToRolesAsync(user,roles);
            }
        }

        public async Task<List<string>> GetRolesToUser(string userIdOrName)
        {
           var user =  await _userManager.FindByIdAsync(userIdOrName);

            if(user == null)
                user = await _userManager.FindByNameAsync(userIdOrName);

            if(user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return roles.ToList();
            }
            return null;
        }

        public async Task<bool> HasRolePermissonToEndpointAsync(string name, string code)
        {
            var roles = await GetRolesToUser(name);
            if (!roles.Any())
                return false;

            Endpoint? endpoint = await _endpointReadRepository.Table.Include(e => e.AppRoles)
                                .FirstOrDefaultAsync(e => e.Code == code);
            if (endpoint == null)
                return false;

            var hasRole = false;
            var endpointRoles = endpoint.AppRoles.Select(x => x.Name);

            foreach (var userRole in roles)
            {
                foreach (var endpointRole in endpointRoles)
                    if (userRole == endpointRole)
                        return true;
            }

            return false;
        }
    }
}
