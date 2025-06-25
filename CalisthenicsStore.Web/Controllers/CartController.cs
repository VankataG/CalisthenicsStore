using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using CalisthenicsStore.Data;
using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Services.Interfaces;
using CalisthenicsStore.ViewModels.CartItem;

namespace CalisthenicsStore.Web.Controllers
{
    public class CartController(ICartService cartService) : BaseController
    {
       
        public async Task<IActionResult> Index()
        {
            
            IEnumerable<CartItemViewModel> model = await cartService.GetCartProductsDataAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            await cartService.AddToCartAsync(productId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Remove(int productId)
        {
            cartService.RemoveFromCart(productId);
            return RedirectToAction("Index");
        }

    }
}
