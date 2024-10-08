using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.Exceptions;
using ETicaretAPI.Application.Features.Commands.AppUser.LoginUser;
using ETicaretAPI.Application.Helpers;
using ETicaretAPI.Domain.Entities.Identity;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Apis.Auth.GoogleJsonWebSignature;
using static Google.Apis.Auth.JsonWebSignature;
using ET = ETicaretAPI.Domain.Entities.Identity;

namespace ETicaretAPI.Persistence.Services
{
    public class AuthService : IAuthService
    {
        readonly UserManager<ETicaretAPI.Domain.Entities.Identity.AppUser> _userManager;
        readonly IConfiguration _configuration;
        readonly ITokenHandler _tokenHandler;
        readonly SignInManager<ET.AppUser> _signInManager;
        readonly IUserService _userService;
        readonly IMailService _mailService;
        public AuthService(
            IConfiguration configuration,
            UserManager<Domain.Entities.Identity.AppUser> userManager,
            ITokenHandler tokenHandler,
            SignInManager<AppUser> signInManager,
            IUserService userService,
            IMailService mailService)
        {
            _configuration = configuration;
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _signInManager = signInManager;
            _userService = userService;
            _mailService = mailService;
        }
        private async Task<Token> CreateUserExternalAsync(AppUser user, string email, string name, UserLoginInfo info,int AccessTokenLifeTime)
        {
            bool result = user != null;
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = email,
                        UserName = email,
                        NameSurname = name
                    };
                    var identityUser = await _userManager.CreateAsync(user);
                    result = identityUser.Succeeded;
                }
            }

            if (result)
            {
                await _userManager.AddLoginAsync(user, info);
                Token token = _tokenHandler.CreateAccessToken(AccessTokenLifeTime, user);
                await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 300);

                return token;
            }
            else
            {
                throw new Exception("Invalid external authentication.");
            }

        }
        public async Task<Token> GoogleLoginAsync(string IdToken, int AccessTokenLifeTime)
        {
            ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { _configuration["ExternalLoginSettings:Google:ClientId"] }
            };
            GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(IdToken, settings);

            var info = new UserLoginInfo("GOOGLE", payload.Subject,"GOOGLE");
            ET.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            return await CreateUserExternalAsync(user, payload.Email, payload.Name, info, AccessTokenLifeTime);
        }

        public async Task<Token> LoginAsync(string UsernameOrEmail, string password, int AccessTokenLifeTime)
        {
            ET.AppUser user = await _userManager.FindByNameAsync(UsernameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(UsernameOrEmail);
            }

            if (user == null)
            {
                throw new NotFoundUserException("Username and Email is not found");
            }

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (result.Succeeded) // authentication is succeeded
            {
                // Yetkileri belirle.
                Token token = _tokenHandler.CreateAccessToken(AccessTokenLifeTime, user);
                await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 300);
                return token;
            }

            //return new LoginUserErrorCommandResponse()
            //{
            //    Message =""
            //};
            throw new AuthenticatonErrorException();
        }

        public async Task<Token> RefreshTokenLoginAsync(string refreshToken)
        {
            AppUser? user =  await _userManager.Users.FirstOrDefaultAsync( user => user.RefreshToken == refreshToken);

            if (user != null && user?.RefreshTokenEndDate > DateTime.UtcNow)
            {
                Token token = _tokenHandler.CreateAccessToken(900, user);
                await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 300);
                return token;
            }
            else
                throw new NotFoundUserException();

        }

        public async Task PasswordResetAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                resetToken = resetToken.UrlEncode();

                await _mailService.SendPasswordResetAsync(email,user.Id,resetToken);
            }

        }

        public async Task<bool> VerifyResetToken(string resetToken, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user != null)
            {
                resetToken = resetToken.UrlDecode();

                return await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", resetToken);
            }
            return false;

        }
    }
}
