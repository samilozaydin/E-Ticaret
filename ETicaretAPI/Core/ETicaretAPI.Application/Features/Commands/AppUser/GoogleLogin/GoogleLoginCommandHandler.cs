using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Abstractions.Services.Authentications;
using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;

using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUser.GoogleLogin
{
    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommandRequest, GoogleLoginCommandResponse> 
    {

        readonly IExternalAuthentication _authService;

        public GoogleLoginCommandHandler(IExternalAuthentication authService)
        {
            _authService = authService;
        }


        public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginCommandRequest request, CancellationToken cancellationToken)
        {
           Token token = await _authService.GoogleLoginAsync(request.IdToken, 900);

            return new()
            {
                Token = token
            };
        }
    }
}
