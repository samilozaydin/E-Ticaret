using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.ViewModels.Basket;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Services
{
    public class BasketService : IBasketService
    {
        readonly IHttpContextAccessor _httpContextAcessor;
        readonly UserManager<AppUser> _userManager;
        readonly IOrderReadRepository _orderReadRepository;
        readonly IBasketWriteRepository _basketWriteRepository;
        readonly IBasketReadRepository _basketReadRepository;

        readonly IBasketItemWriteRepository _basketItemWriteRepository;
        readonly IBasketItemReadRepository _basketItemReadRepository;

        public BasketService(IHttpContextAccessor httpContextAcessor, UserManager<AppUser> userManager, IOrderReadRepository orderReadRepository, IBasketWriteRepository basketWriteRepository, IBasketItemWriteRepository basketItemWriteRepository, IBasketItemReadRepository basketItemReadRepository, IBasketReadRepository basketReadRepository)
        {
            _httpContextAcessor = httpContextAcessor;
            _userManager = userManager;
            _orderReadRepository = orderReadRepository;
            _basketWriteRepository = basketWriteRepository;
            _basketItemWriteRepository = basketItemWriteRepository;
            _basketItemReadRepository = basketItemReadRepository;
            _basketReadRepository = basketReadRepository;
        }

        private async Task<Basket?> ContextUser()
        {
            var username = _httpContextAcessor.HttpContext?.User?.Identity?.Name;
            if(!string.IsNullOrEmpty(username))
            {
                AppUser? user = await _userManager.Users.
                    Include(user => user.Baskets)
                    .FirstOrDefaultAsync(user => user.UserName == username);

                var basketCollection = from basket in user.Baskets
                                       join orders in _orderReadRepository.Table
                                       on basket.Id equals orders.Id into OrderBasket
                                       from orders in OrderBasket.DefaultIfEmpty()
                                       select new
                                       {
                                           Basket = basket,
                                           Order = orders
                                       };
                Basket? targetBasket = null;
                if (basketCollection.Any(b => b.Order is null)) {
                    targetBasket = basketCollection.FirstOrDefault(b => b.Order is null)?.Basket;
                }
                else
                {
                    targetBasket = new();
                    user.Baskets.Add(targetBasket);
                }
                await _basketWriteRepository.SaveAsync();
                return targetBasket;

            }throw new Exception("When getting Basket, username is null.");



        }
        [HttpPost]
        public async Task AddItemToBasket(VM_Create_BasketItem basketItem)
        {
            var basket = await ContextUser();
            if(basket != null)
            {
                BasketItem? _basketItem = await _basketItemReadRepository
                    .GetSingleAsync(bi => bi.ProductId == Guid.Parse(basketItem.ProductId)
                                       && bi.BasketId == basket.Id);
                if (_basketItem != null)
                    _basketItem.Quantity += basketItem.Quantity;
                else
                    await _basketItemWriteRepository.AddAsync(new()
                    {
                        ProductId = Guid.Parse(basketItem.ProductId),
                        Quantity = basketItem.Quantity,
                        BasketId = basket.Id
                    });
                
                await _basketItemWriteRepository.SaveAsync();
            }

        }
        [HttpGet]

        public async Task<List<BasketItem>> GetBasketItemsAsync()
        {
            Basket? UserBasket = await ContextUser();
            Basket? userBasketNavigated = await _basketReadRepository.Table
                .Include(b => b.BasketItems)
                .ThenInclude(bi => bi.Product)
                .FirstOrDefaultAsync(b => b.Id == UserBasket.Id);
            
            return userBasketNavigated.BasketItems.ToList();
        
        }
        [HttpDelete]

        public async Task RemoveBasketItem(string basketItemId)
        {
            BasketItem? basketItem = await _basketItemReadRepository.GetByIdAsync(basketItemId);
            if (basketItem != null)
            {
                _basketItemWriteRepository.Remove(basketItem);
                await _basketItemWriteRepository.SaveAsync();
            }
        }
        [HttpPut]
        public async Task UpdateBasketItem(VM_Update_BasketItem basketItem)
        {
            BasketItem? _basketItem = await _basketItemReadRepository.GetByIdAsync(basketItem.BasketItemId);
            if(_basketItem != null)
            {
                _basketItem.Quantity = basketItem.Quantity;
                await _basketItemWriteRepository.SaveAsync();
            }
        }

        public async Task<Basket> GetUserActiveBasket()
        {
            Basket? basket = await ContextUser();
            return basket;
        }
    }
}
