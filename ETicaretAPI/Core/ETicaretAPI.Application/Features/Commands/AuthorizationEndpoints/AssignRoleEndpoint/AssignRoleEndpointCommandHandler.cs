using ETicaretAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AuthorizationEndpoints.AssignRole
{
    public class AssignRoleEndpointCommandHandler : IRequestHandler<AssignRoleEndpointCommandRequest, AssignRoleEndpointCommandResponse>
    {
        readonly IAutherizationEndpointService _autherizationEndpointService;

        public AssignRoleEndpointCommandHandler(IAutherizationEndpointService autherizationEndpointService)
        {
            _autherizationEndpointService = autherizationEndpointService;
        }

        public async Task<AssignRoleEndpointCommandResponse> Handle(AssignRoleEndpointCommandRequest request, CancellationToken cancellationToken)
        {
            await _autherizationEndpointService.AssignRoleEndpointsAsync(request.Roles, request.Menu, request.Code, request.Type);

            return new()
            {

            };


        }
    }
}
