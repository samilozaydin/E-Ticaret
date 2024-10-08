using ETicaretAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.Basket.UpdateBasketItem
{
    public class UpdateBasketItemCommandHandler : IRequestHandler<UpdateBasketItemCommandRequest, UpdateBasketItemCommandResponse>
    {
        readonly IBasketService _basketService;

        public UpdateBasketItemCommandHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }
        public async Task<UpdateBasketItemCommandResponse> Handle(UpdateBasketItemCommandRequest request, CancellationToken cancellationToken)
        {
            await _basketService.UpdateBasketItem(new()
            {
                BasketItemId = request.BasketItemId,
                Quantity = request.Quantity,
            });
            return new();
        }
    }
}
