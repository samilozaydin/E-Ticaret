using ETicaretAPI.Application.DTOs.Product;
using ETicaretAPI.Application.Features.Queries.Order.GetAllOrders;
using ETicaretAPI.Application.ViewModels.Basket;
using ETicaretAPI.Application.ViewModels.Products;
using ETicaretAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions.Services
{
    public interface IProductService
    {
        Task<List<ProductGetAllDTO>> GetAllProducts(int page, int size);
        Task<ProductGetById> GetProductById(string id);
        Task CreateProduct(VM_Create_Product product);
        Task UpdateProduct(VM_Update_Product product);
        Task DeleteProduct(string id);
        Task<int> ProductsCount();
    }
}
