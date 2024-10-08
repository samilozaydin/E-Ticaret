using ETicaretAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.AuthorizationEndpoints.GetRolesToEndpoints
{
    public class GetRolesToEndpointQueryHandler : IRequestHandler<GetRolesToEndpointQueryRequest, GetRolesToEndpointQueryResponse>
    {
        readonly IAutherizationEndpointService _autherizationEndpointService;

        public GetRolesToEndpointQueryHandler(IAutherizationEndpointService autherizationEndpointService)
        {
            _autherizationEndpointService = autherizationEndpointService;
        }

        public async Task<GetRolesToEndpointQueryResponse> Handle(GetRolesToEndpointQueryRequest request, CancellationToken cancellationToken)
        {
          var data = await _autherizationEndpointService.GetRolesToEndpoint(request.Code,request.Menu);
            return new()
            {
                Roles = data
            };
        }

    }
}
