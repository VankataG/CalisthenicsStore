using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using CalisthenicsStore.Data;
using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Data.Repositories.Interfaces;
using CalisthenicsStore.Services.Interfaces;
using CalisthenicsStore.ViewModels.CartItem;

namespace CalisthenicsStore.Services
{
    public class CartService : ICartService
    {
        private readonly IProductRepository productRepository;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CartService(IProductRepository productRepository, IHttpContextAccessor httpContextAccessor)
        {
            this.productRepository = productRepository;
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

        public async Task<IEnumerable<CartItemViewModel>> GetCartProductsDataAsync()
        {
            List<CartItem> cartItems = this.GetCart();
            List<int> productIds = cartItems.Select(ci => ci.ProductId).ToList();

            Dictionary<int, Product> products = await productRepository
                .GetAllAttacked()
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id);

            IEnumerable<CartItemViewModel> model = cartItems.Select(ci => new CartItemViewModel()
                {
                    ProductId = ci.ProductId,
                    ProductName = products[ci.ProductId].Name,
                    ImageUrl = products[ci.ProductId].ImageUrl,
                    Price = products[ci.ProductId].Price,
                    Quantity = ci.Quantity
                })
                .ToList();

            return model;

        }

        public void SaveCart(List<CartItem> cart)
        {
            var session = httpContextAccessor.HttpContext!.Session;
            var cartJson = JsonSerializer.Serialize(cart);
            session.SetString("Cart", cartJson);
        }

        public async Task AddToCartAsync(int productId)
        {
            var product = await productRepository
                .FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null) return;

            var cart = GetCart();

            var existingItem = cart.FirstOrDefault(c => c.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    Quantity = 1
                });
            }

            SaveCart(cart);
        }

        public void RemoveFromCart(int productId)
        {
            var cart = GetCart();
            var itemToRemove = cart.FirstOrDefault(c => c.ProductId == productId);
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
