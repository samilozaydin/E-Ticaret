using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        IProductReadRepository _productReadRepository;
        IProductWriteRepository _productWriteRepository;
        public ProductsController(IProductReadRepository productReadRepository,
        IProductWriteRepository productWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
        } 
        [HttpGet]
        public async Task Get() {
            //await _productWriteRepository.AddRangeAsync(new()
            //{
            //   new() { Id = Guid.NewGuid(),Name="Product 1",Price =100, CreatedDate =DateTime.UtcNow, Stock=10 },
            //   new() { Id = Guid.NewGuid(),Name="Product 2",Price =200, CreatedDate = DateTime.UtcNow, Stock=20 }
            //});
            //int count = await _productWriteRepository.SaveAsync();
            Product product = await _productReadRepository.GetByIdAsync("8b6b28b6-0171-41e0-a03d-db036f7df25a",false);
            product.Name = "bilal";
            await _productWriteRepository.SaveAsync();
        }
    }
}
