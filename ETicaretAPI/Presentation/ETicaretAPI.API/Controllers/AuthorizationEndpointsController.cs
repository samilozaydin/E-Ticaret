using ETicaretAPI.Application.Consts;
using ETicaretAPI.Application.CustomAttributes;
using ETicaretAPI.Application.Enums;
using ETicaretAPI.Application.Features.Commands.AuthorizationEndpoints.AssignRole;
using ETicaretAPI.Application.Features.Queries.AuthorizationEndpoints.GetRolesToEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationEndpointsController : ControllerBase
    {
        readonly IMediator _mediator;

        public AuthorizationEndpointsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("get-roles-to-endpoint")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.AuthorizationEndpoints, ActionType = ActionType.Reading, Definition = "Get Basket Items")]

        public async Task<IActionResult> GetRolesToEndpoint(GetRolesToEndpointQueryRequest request)
        {
            GetRolesToEndpointQueryResponse response = await _mediator.Send(request);

            return Ok(response);
        }

        [HttpPost]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.AuthorizationEndpoints, ActionType = ActionType.Writing, Definition = "Get Basket Items")]
        public async Task<IActionResult> AssingRole(AssignRoleEndpointCommandRequest request)
        {
            request.Type = typeof(Program);
            AssignRoleEndpointCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
