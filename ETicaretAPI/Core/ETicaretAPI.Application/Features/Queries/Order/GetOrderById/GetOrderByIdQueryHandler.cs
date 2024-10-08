using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.Order;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.Order.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQueryRequest, GetOrderByIdQueryResponse>
    {
        readonly IOrderService _orderService;

        public GetOrderByIdQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<GetOrderByIdQueryResponse> Handle(GetOrderByIdQueryRequest request, CancellationToken cancellationToken)
        {
           SingleOrder order =  await _orderService.GetOrderById(request.Id);

            return new()
            {
                Address= order.Address,
                BasketItems = order.BasketItems,
                CreatedDate = order.CreatedDate,
                Description= order.Description,
                Id = order.Id,
                OrderCode = order.OrderCode,
                Completed = order.Completed
            };
        }
    }
}
