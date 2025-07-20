using Microsoft.AspNetCore.Mvc;

using CalisthenicsStore.Data.Models;
using CalisthenicsStore.ViewModels.CartItem;

namespace CalisthenicsStore.Services.Interfaces
{
    public interface ICartService
    {
        List<CartItem> GetCart();

        Task<IEnumerable<CartItemViewModel>> GetCartProductsDataAsync();

        void SaveCart(List<CartItem> cart);

        Task AddToCartAsync(Guid productId);

        void RemoveFromCart(Guid productId);

        void ClearCart();
    }
}
