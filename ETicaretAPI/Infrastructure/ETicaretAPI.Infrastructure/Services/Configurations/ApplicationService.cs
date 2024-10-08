using ETicaretAPI.Application.Abstractions.Services.Configurations;
using ETicaretAPI.Application.CustomAttributes;
using ETicaretAPI.Application.DTOs.Configuration;
using ETicaretAPI.Application.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services.Configurations
{
    public class ApplicationService : IApplicationService
    {
        public List<Menu> GetAuthorazitonDefinitionEndpoints(Type assemblyType)
        {
            List<Menu> menus = new();

            Assembly assembly = Assembly.GetAssembly(assemblyType);
            var controllers = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(ControllerBase)));
            if (controllers != null)
            {
                foreach (var controller in controllers)
                {
                    var actions = controller.GetMethods().Where(c => c.IsDefined(typeof(AuthorizeDefinitionAttribute)));
                    if(actions != null)
                    {
                        foreach (var action in actions)
                        {

                            var attributes =  action.GetCustomAttributes(true);
                            if(attributes != null)
                            {
                                Menu menu = null;

                                var authorizeDefinitionAttr = attributes.FirstOrDefault(t => t.GetType() == typeof(AuthorizeDefinitionAttribute)) as AuthorizeDefinitionAttribute;
                                if (!menus.Any(m => m.Name == authorizeDefinitionAttr.Menu))
                                {
                                    menu = new Menu { Name = authorizeDefinitionAttr.Menu };
                                    menus.Add(menu);
                                }
                                else
                                    menu = menus.FirstOrDefault(m => m.Name == authorizeDefinitionAttr.Menu);

                                Application.DTOs.Configuration.Action _action = new()
                                {
                                    ActionType = Enum.GetName(typeof(ActionType),authorizeDefinitionAttr.ActionType),
                                    Definition = authorizeDefinitionAttr.Definition
                                };

                                var httpMethodAttr = attributes.FirstOrDefault(attr => attr.GetType().IsAssignableTo(typeof(HttpMethodAttribute))) as HttpMethodAttribute;
                                if (httpMethodAttr != null)
                                    _action.HttpType = httpMethodAttr.HttpMethods.First();
                                else
                                    _action.HttpType = HttpMethods.Get;

                                _action.Code = $"{_action.HttpType}.{_action.ActionType}.{_action.Definition.Replace(" ","")}";

                                menu.Actions.Add(_action);
                            }

                        }
                    }
                }

            }
            return menus;
        }
    }
}
