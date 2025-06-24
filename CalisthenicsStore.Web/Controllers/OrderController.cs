using CalisthenicsStore.Services.Interfaces;
using CalisthenicsStore.ViewModels.Order;
using Microsoft.AspNetCore.Mvc;

namespace CalisthenicsStore.Web.Controllers
{
    public class OrderController(IOrderService orderService) : BaseController
    {
        [HttpGet]
        public IActionResult Checkout()
        {
            CheckoutViewModel model = orderService.CheckoutCartItems();
           

            return View(model);
        }

        //[HttpPost]
        //public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
        //{

        //}

    }
}
