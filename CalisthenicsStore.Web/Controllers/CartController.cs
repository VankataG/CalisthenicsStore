
using Microsoft.AspNetCore.Mvc;

using CalisthenicsStore.Services.Interfaces;
using CalisthenicsStore.ViewModels.CartItem;
using static CalisthenicsStore.Common.Constants.Notifications;

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
        public async Task<IActionResult> AddToCart(Guid productId, string returnUrl)
        {
            try
            {
                await cartService.AddToCartAsync(productId);
                TempData[SuccessMessageKey] = "Successfully added to cart!";
            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "Failed adding to cart!";
            }

            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            
            return RedirectToAction("Details", "Product", new { id = productId});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCartAjax(Guid productId)
        {
            try
            {
                await cartService.AddToCartAsync(productId);

                int cartCount = cartService.GetCart().Count();

                TempData[SuccessMessageKey] = "Successfully added to cart!";
                return Json(new
                {
                    success = true,
                    message = "Successfully added to cart!",
                    cartCount
                });
            }
            catch 
            {
                TempData[ErrorMessageKey] = "Failed adding to cart!";

                return Json(new
                {
                    success = false,
                    message = "Failed adding to cart!"
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Remove(Guid productId)
        {
            try
            {
                await cartService.RemoveFromCart(productId);
                TempData[SuccessMessageKey] = "Successfully removed from cart!";
            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "Failed to remove from cart!";
            }


            return RedirectToAction(nameof(Index));
        }

    }
}
