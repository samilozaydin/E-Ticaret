using ETicaretAPI.Application.Abstractions.Cache;
using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.Product.RemoveProduct
{
    public class RemoveProductCommandHandler : IRequestHandler<RemoveProductCommandRequest, RemoveProductCommandResponse>
    {
        private readonly IProductService _productService;
        private readonly ICacheService _cacheService;

        public RemoveProductCommandHandler(IProductService productService, ICacheService cacheService)
        {
            _productService = productService;
            _cacheService = cacheService;
        }

        public async Task<RemoveProductCommandResponse> Handle(RemoveProductCommandRequest request, CancellationToken cancellationToken)
        {
            await _productService.DeleteProduct(request.Id);
            var keys = _cacheService.GetKeys();
            foreach (var key in keys)
                _cacheService.Remove(key);
            return new();
        }
    }
}
