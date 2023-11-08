using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.RequestParameters;
using ETicaretAPI.Application.ViewModels.Products;
using ETicaretAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        public async Task<IActionResult> Get([FromQuery] Pagination pagination) {
            //await _productWriteRepository.AddRangeAsync(new()
            //{
            //   new() { Id = Guid.NewGuid(),Name="Product 3",Price =100,  Stock=10 },
            //   new() { Id = Guid.NewGuid(),Name="Product 4",Price =200,  Stock=20 }
            //});
            //int count = await _productWriteRepository.SaveAsync();
            //Product product = await _productReadRepository.GetByIdAsync("8b6b28b6-0171-41e0-a03d-db036f7df25a",false);
            //product.Name = "bilal";
            //await _productWriteRepository.SaveAsync();
            var totalCount = _productReadRepository.GetAll(false).Count();
            var products = _productReadRepository.GetAll(false).Select(p => new
            {
                p.Id,
                p.Name,
                p.Price,
                p.Stock,
                p.CreatedDate,
                p.UpdatedDate
            }).Skip(pagination.Page*pagination.Size).Take(pagination.Size).ToList();
            return Ok(new { products, totalCount });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await _productReadRepository.GetByIdAsync(id,false));
        }
        [HttpPost]
        public async Task<IActionResult> Post(VM_Create_Product model)
        {
            await _productWriteRepository.AddAsync(new()
            {
                Stock = model.Stock,
                Name = model.Name,
                Price = model.Price
            });
            await _productWriteRepository.SaveAsync();
            return StatusCode((int)HttpStatusCode.Created);
        }
        [HttpPut]
        public async Task<IActionResult> Put(VM_Update_Product model)
        {
            Product product = await _productReadRepository.GetByIdAsync(model.Id);
            product.Name = model.Name;
            product.Price = model.Price;
            product.Stock = model.Stock;
            await _productWriteRepository.SaveAsync();
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _productWriteRepository.Remove(id);
            await _productWriteRepository.SaveAsync();
            return Ok();
        }
    }
}
