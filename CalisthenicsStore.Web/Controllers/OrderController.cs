using System.Security.Claims;
using CalisthenicsStore.Services.Interfaces;
using CalisthenicsStore.ViewModels.Order;
using Microsoft.AspNetCore.Mvc;

namespace CalisthenicsStore.Web.Controllers
{
    public class OrderController(IOrderService orderService) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            CheckoutViewModel model = await orderService.CheckoutCartItemsAsync();
           

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Checkout", model);
            }

            string email = User.FindFirstValue(ClaimTypes.Email);

            int orderId = await orderService.PlaceOrderAsync(model, email);

            return RedirectToAction("Confirmation", new { id = orderId });
        }

        public IActionResult ThankYou()
        {
            return View();
        }
    }
}
