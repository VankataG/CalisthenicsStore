using Microsoft.AspNetCore.Mvc;

namespace CalisthenicsStore.Web.Areas.Admin.Controllers
{
    public class HomeController : BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
