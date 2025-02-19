using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.Product;
using ETicaretAPI.Application.Features.Queries.Product.GetByIdProduct;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.ViewModels.Products;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Persistence.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Services
{
    public class ProductService : IProductService
    {
        readonly IProductWriteRepository _productWriteRepository;
        readonly IProductReadRepository _productReadRepository;

        public ProductService(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
        }

        public async Task CreateProduct(VM_Create_Product product)
        {
            await _productWriteRepository.AddAsync(new()
            {
                Stock = product.Stock,
                Name = product.Name,
                Price = product.Price
            });
            await _productWriteRepository.SaveAsync();
        }

        public async Task DeleteProduct(string id)
        {
            await _productWriteRepository.Remove(id);
            await _productWriteRepository.SaveAsync();
        }

        public  async Task<List<ProductGetAllDTO>> GetAllProducts(int page, int size)
        {
            var products = _productReadRepository.GetAll(false).Skip(page*size).Take(size)
                .Include(products => products.ProductImageFiles)
                .Select(p => new ProductGetAllDTO
                {
                   Id= p.Id,
                   Name=p.Name,
                    Price = p.Price,
                    Stock = p.Stock,
                    CreatedDate = p.CreatedDate,
                    UpdatedDate = p.UpdatedDate,
                    ProductImageFiles= p.ProductImageFiles
                }).ToList();
            return products;
        }

        public async Task<ProductGetById> GetProductById(string id)
        {
            Product product = await _productReadRepository.GetByIdAsync(id, false);
            var result = new ProductGetById{
                Name = product.Name,
                Price= product.Price,
                Stock = product.Stock
            };
            return result;
        }

        public async Task<int> ProductsCount()
        {
            return _productReadRepository.GetAll(false).Count();
        }

        public async Task UpdateProduct(VM_Update_Product product)
        {
            var item = await _productReadRepository.GetByIdAsync(product.Id);
            item.Name = product.Name;
            item.Price = product.Price;
            item.Stock = product.Stock;
            await _productWriteRepository.SaveAsync();
        }
    }
}
