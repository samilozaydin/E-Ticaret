using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Consts;
using ETicaretAPI.Application.CustomAttributes;
using ETicaretAPI.Application.Enums;
using ETicaretAPI.Application.Features.Commands.AppUser.AssignRoleToUser;
using ETicaretAPI.Application.Features.Commands.AppUser.CreateUser;
using ETicaretAPI.Application.Features.Commands.AppUser.GoogleLogin;
using ETicaretAPI.Application.Features.Commands.AppUser.LoginUser;
using ETicaretAPI.Application.Features.Commands.AppUser.UpdatePassword;
using ETicaretAPI.Application.Features.Queries.AppUser.GetAllUsers;
using ETicaretAPI.Application.Features.Queries.AuthorizationEndpoints.GetRolesToUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserCommandRequest request)
        {
            CreateUserCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody]  UpdatePasswordCommandRequest request)
        {
            UpdatePasswordCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes ="Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Users, ActionType = ActionType.Reading, Definition = "Get All Users")]

        public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUsersQueryRequest request)
        {
            GetAllUsersQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpPost("assign-role-to-user")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Users, ActionType = ActionType.Writing, Definition = "Assign Role To User")]

        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleToUserCommandRequest request)
        {
            AssignRoleToUserCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpGet("get-roles-to-user/{UserId}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Users, ActionType = ActionType.Reading, Definition = "Get Roles To User")]

        public async Task<IActionResult> GetRolesToUser([FromRoute] GetRolesToUserQueryRequest request)
        {
            GetRolesToUserQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
