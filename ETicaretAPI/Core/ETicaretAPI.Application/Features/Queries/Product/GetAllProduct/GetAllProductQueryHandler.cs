using ETicaretAPI.Application.Abstractions.Cache;
using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.Caches;
using ETicaretAPI.Application.DTOs.Product;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.RequestParameters;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.Product.GetAllProduct
{
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQueryRequest, GetAllProductQueryResponse>
    {
        readonly IProductService _productService;
        private readonly ICacheService _cacheService;

        public GetAllProductQueryHandler(IProductService productService, ICacheService cacheService)
        {
            _productService = productService;
            _cacheService = cacheService;
        }

        public async Task<GetAllProductQueryResponse> Handle(GetAllProductQueryRequest request, CancellationToken cancellationToken)
        {
            if (_cacheService.Contains($"products_{request.Page}_{request.Size}"))
            {
                var cachedProducts = _cacheService.Get($"products_{request.Page}_{request.Size}");
                //var cachedItemJSON = cachedProducts.GetValue<JsonElement>();
                var cachedItem = JsonSerializer.Deserialize<CachedProductGetAllDTO>(cachedProducts.Value.ToString());

                return new GetAllProductQueryResponse { Products = cachedItem.Products, TotalProductCount = cachedItem.ProductsCount };
            }

            var totalCount = await _productService.ProductsCount();
            var products = await _productService.GetAllProducts(request.Page,request.Size);
            
            _cacheService.Set($"products_{request.Page}_{request.Size}",
                new CachedProductGetAllDTO { ProductsCount=totalCount,Products = products},
                TimeSpan.FromMinutes(15));

            return new GetAllProductQueryResponse { Products = products, TotalProductCount = totalCount };
        }
    }
}
