using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using CalisthenicsStore.Data;
using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Services.Interfaces;

namespace CalisthenicsStore.Services
{
    public class CartService : ICartService
    {
        private readonly CalisthenicsStoreDbContext context;

        private readonly IHttpContextAccessor httpContextAccessor;

        public CartService(CalisthenicsStoreDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.httpContextAccessor = httpContextAccessor;
        }


        public List<CartItem> GetCart()
        {

            var session = httpContextAccessor.HttpContext!.Session;
            var cartJson = session.GetString("Cart");
            return string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(cartJson)!;
        }

        public void SaveCart(List<CartItem> cart)
        {
            var session = httpContextAccessor.HttpContext!.Session;
            var cartJson = JsonSerializer.Serialize(cart);
            session.SetString("Cart", cartJson);
        }

        public async Task AddToCartAsync(int productId)
        {
            var product = await context.Products.FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null) return;

            var cart = GetCart();

            var existingItem = cart.FirstOrDefault(c => c.Product.Id == productId);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    Product = product,
                    Quantity = 1
                });
            }

            SaveCart(cart);
        }

        public void RemoveFromCart(int productId)
        {
            var cart = GetCart();
            var itemToRemove = cart.FirstOrDefault(c => c.Product.Id == productId);
            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                SaveCart(cart);
            }
        }


        public void ClearCart()
        {
            var session = httpContextAccessor.HttpContext!.Session;
            session.Remove("Cart");
        }

    }
}
