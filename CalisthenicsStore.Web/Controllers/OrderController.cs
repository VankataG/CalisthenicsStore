using Microsoft.AspNetCore.Mvc;

namespace CalisthenicsStore.Web.Controllers
{
    public class OrderController : BaseController
    {
        public IActionResult Checkout()
        {
            return View();
        }
    }
}
