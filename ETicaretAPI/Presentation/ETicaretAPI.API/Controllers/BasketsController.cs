using Azure.Core;
using ETicaretAPI.Application.Consts;
using ETicaretAPI.Application.CustomAttributes;
using ETicaretAPI.Application.Enums;
using ETicaretAPI.Application.Features.Commands.Basket.AddItemToBasket;
using ETicaretAPI.Application.Features.Commands.Basket.RemoveBasketItem;
using ETicaretAPI.Application.Features.Commands.Basket.UpdateBasketItem;
using ETicaretAPI.Application.Features.Queries.Basket.GetBasketItems;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes ="Admin")]
    public class BasketsController : ControllerBase
    {
        readonly IMediator _mediator;

        public BasketsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Baskets,ActionType = ActionType.Reading, Definition ="Get Basket Items")]
        public async Task<IActionResult> GetBasketItems([FromQuery]GetBasketItemsQueryRequest request)
        {
            List<GetBasketItemsQueryResponse> response = await _mediator.Send(request);
            return Ok(response); 
        }
        [HttpPost]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Baskets, ActionType = ActionType.Writing, Definition = "Add Item To Basket")]

        public async Task<IActionResult> AddItemToBasket(AddItemToBasketCommandRequest request)
        {
            AddItemToBasketCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpPut]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Baskets, ActionType = ActionType.Updating, Definition = "Update Quantity")]

        public async Task<IActionResult> UpdateQuantity(UpdateBasketItemCommandRequest request)
        {
            UpdateBasketItemCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete("{BasketItemId}")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Baskets, ActionType = ActionType.Deleting, Definition = "Remove Basket Item")]

        public async Task<IActionResult> RemoveBasketItem([FromRoute]RemoveBasketItemCommandRequest request)
        {
            RemoveBasketItemCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }


    }
}
