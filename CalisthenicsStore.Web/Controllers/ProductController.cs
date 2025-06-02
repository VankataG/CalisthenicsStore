using Microsoft.AspNetCore.Mvc;

using CalisthenicsStore.Services.Interfaces;

namespace CalisthenicsStore.Web.Controllers
{
    public class ProductController(IProductService service) : Controller
    {
       
        public async Task<IActionResult> Index()
        {
            var products = await service.GetAllAsync();

            return View(products);
        }


        public async Task<IActionResult> Category(int id)
        {
            var products = await service.GetByCategoryAsync(id);

            return View("Index", products); // Reuse Index view
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await service.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

    }
}
