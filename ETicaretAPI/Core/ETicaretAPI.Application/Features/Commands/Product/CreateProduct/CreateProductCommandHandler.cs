using ETicaretAPI.Application.Abstractions.Cache;
using ETicaretAPI.Application.Abstractions.Hubs;
using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.Product.CreateProduct
{
    internal class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
    {
        private readonly IProductService _productService;
        readonly IProductHubService _productHubService;
        private readonly ICacheService _cacheService;
        public CreateProductCommandHandler(IProductService productService, IProductHubService productHubService, ICacheService cacheService)
        {
            _productService = productService;
            _productHubService = productHubService;
            _cacheService = cacheService;
        }

        public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {
            await _productService.CreateProduct(new()
            {
                Stock = request.Stock,
                Name = request.Name,
                Price = request.Price
            });

            await _productHubService.ProductAddedMessageAsync($"Product named {request.Name} is added");

            var keys = _cacheService.GetKeys();
            foreach ( var key in keys ) 
                _cacheService.Remove( key );
            

            return new() { IsSuccess = true };
        }
    }
}
