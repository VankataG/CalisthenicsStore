using Microsoft.AspNetCore.Mvc;

using CalisthenicsStore.Data.Models;

namespace CalisthenicsStore.Services.Interfaces
{
    public interface ICartService
    {
        List<CartItem> GetCart();

        void SaveCart(List<CartItem> cart);

        Task AddToCartAsync(int productId);

        void RemoveFromCart(int productId);
    }
}
