using ETicaretAPI.Application.Abstractions.Services.Configurations;
using ETicaretAPI.Application.CustomAttributes;
using ETicaretAPI.Application.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes ="Admin")]
    public class ApplicationServiceController : ControllerBase
    {
        readonly IApplicationService _applicationService;

        public ApplicationServiceController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }
        [HttpGet]
        [AuthorizeDefinition(ActionType =ActionType.Reading,Menu ="Application Service",Definition ="Get Authorization Definition Endpoints")]
        public IActionResult GetAutherizationDefinitionEndpoints()
        {
            var data = _applicationService.GetAuthorazitonDefinitionEndpoints(typeof(Program));
            return Ok(data);
        }
    }
}
