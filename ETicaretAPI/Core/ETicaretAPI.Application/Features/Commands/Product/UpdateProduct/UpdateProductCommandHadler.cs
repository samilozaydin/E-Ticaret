using ETicaretAPI.Application.Abstractions.Cache;
using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ET = ETicaretAPI.Domain.Entities;

namespace ETicaretAPI.Application.Features.Commands.Product.UpdateProduct
{
    public class UpdateProductCommandHadler : IRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
    {
        readonly IProductService _productService;
        private readonly ICacheService _cacheService;

        public UpdateProductCommandHadler(IProductService productService, ICacheService cacheService) : this(productService)
        {
            _cacheService = cacheService;
        }

        public UpdateProductCommandHadler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
        {
            await _productService.UpdateProduct(new ViewModels.Products.VM_Update_Product
            { Id= request.Id,
              Name = request.Name, 
              Price= request.Price,
              Stock= request.Stock}
            );

            var keys = _cacheService.GetKeys();
            foreach (var key in keys)
                _cacheService.Remove(key);

            return new();
        }
    }
}
