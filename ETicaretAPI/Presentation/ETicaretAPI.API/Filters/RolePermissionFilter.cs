using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.CustomAttributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ETicaretAPI.API.Filters
{
    public class RolePermissionFilter : IAsyncActionFilter
    {
        readonly IUserService _userService;

        public RolePermissionFilter(IUserService userService)
        {
            _userService = userService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var name = context.HttpContext.User.Identity.Name;
            if (!string.IsNullOrEmpty(name) && !name.Equals("namenick"))
            {
                var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
                var attr = descriptor.MethodInfo.GetCustomAttribute(typeof(AuthorizeDefinitionAttribute)) as AuthorizeDefinitionAttribute;
                var httpMethodattr = descriptor.MethodInfo.GetCustomAttribute(typeof(HttpMethodAttribute)) as HttpMethodAttribute;

                var code = $"{(httpMethodattr != null ? httpMethodattr.HttpMethods.First() : HttpMethods.Get)}.{attr.ActionType}.{attr.Definition.Replace(" ","")}";

                var hasRole = await _userService.HasRolePermissonToEndpointAsync(name,code);

                if (!hasRole)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
                else
                    await next();
            }
            await next();
        }
    }
}
