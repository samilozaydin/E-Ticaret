using ETicaretAPI.Application.ViewModels.Basket;
using ETicaretAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions.Services
{
    public interface IBasketService
    {
        public Task<List<BasketItem>> GetBasketItemsAsync();
        public Task AddItemToBasket(VM_Create_BasketItem basketItem);
        public Task UpdateBasketItem(VM_Update_BasketItem basketItem);
        public Task RemoveBasketItem(string basketItemId);
        public Task<Basket> GetUserActiveBasket(); 
    }
}
