using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using CalisthenicsStore.Data;
using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Services.Interfaces;

namespace CalisthenicsStore.Web.Controllers
{
    [Authorize]
    public class CartController/*(ICartService cartService)*/ : Controller
    {
        private readonly CalisthenicsStoreDbContext dbContext;
        public CartController(CalisthenicsStoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null) return NotFound();
            var cart = GetCart();

            var existingItem =cart.FirstOrDefault(c => c.Product.Id == productId);
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
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Remove(int productId)
        {
            var cart = GetCart();
            var itemToRemove = cart.FirstOrDefault(c => c.Product.Id == productId);
            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                SaveCart(cart);
            }
            return RedirectToAction("Index");
        }

        private List<CartItem> GetCart()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            return string.IsNullOrEmpty(cartJson)
                ? new List<CartItem>()
                : JsonSerializer.Deserialize<List<CartItem>>(cartJson)!;
        }

        private void SaveCart(List<CartItem> cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString("Cart", cartJson);
        }
    }
}
