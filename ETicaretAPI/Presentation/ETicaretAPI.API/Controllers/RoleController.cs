using Azure.Core;
using ETicaretAPI.Application.Consts;
using ETicaretAPI.Application.CustomAttributes;
using ETicaretAPI.Application.DTOs.Configuration;
using ETicaretAPI.Application.Enums;
using ETicaretAPI.Application.Features.Commands.Role.CreateRole;
using ETicaretAPI.Application.Features.Commands.Role.DeleteRole;
using ETicaretAPI.Application.Features.Commands.Role.UpdateRole;
using ETicaretAPI.Application.Features.Queries.Role.GetRoleById;
using ETicaretAPI.Application.Features.Queries.Role.GetRoles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes ="Admin")]
    public class RoleController : ControllerBase
    {
        readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AuthorizeDefinition(ActionType = ActionType.Reading,Definition ="Get Roles",Menu = AuthorizeDefinitionConstants.Roles)]
        public async Task<IActionResult> GetRoles([FromQuery] GetRolesQueryRequest request)
        {
            GetRolesQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get Roles By Id",Menu = AuthorizeDefinitionConstants.Roles)]

        public async Task<IActionResult> GetRoles([FromRoute]GetRoleByIdQueryRequest request) 
        {
            GetRoleByIdQueryResponse response = await _mediator.Send(request);

            return Ok(response);
        }

        [HttpPost]
        [AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Create Role", Menu = AuthorizeDefinitionConstants.Roles)]

        public async Task<IActionResult> CreateRole([FromBody]CreateRoleCommandRequest request)
        {
            CreateRoleCommandResponse response = await _mediator.Send(request);

            return Ok(response);
        }
        [HttpPut("{id}")]
        [AuthorizeDefinition(ActionType = ActionType.Updating, Definition = "Update Role", Menu = AuthorizeDefinitionConstants.Roles)]


        public async Task<IActionResult> UpdateRole([FromBody,FromRoute]UpdateRoleCommandRequest request)
        {
            UpdateRoleCommandResponse response = await _mediator.Send(request);

            return Ok(response);
        }
        [HttpDelete("{id}")]
        [AuthorizeDefinition(ActionType = ActionType.Deleting, Definition = "Delete Role", Menu = AuthorizeDefinitionConstants.Roles)]


        public async Task<IActionResult> DeleteRole([FromRoute]DeleteRoleCommandRequest request)
        {
            DeleteRoleCommandResponse response = await _mediator.Send(request);

            return Ok(response);
        }
    }
}
