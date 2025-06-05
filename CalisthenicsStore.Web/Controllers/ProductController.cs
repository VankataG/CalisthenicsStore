using Microsoft.AspNetCore.Mvc;

using CalisthenicsStore.Services.Interfaces;
using CalisthenicsStore.ViewModels.Product;

namespace CalisthenicsStore.Web.Controllers
{
    public class ProductController(IProductService service) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await service.GetAllAsync();

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Category(int id)
        {
            var products = await service.GetByCategoryAsync(id);

            return View("Index", products); // Reuse Index view
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await service.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
             
            return View(product);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            AddProductInputModel model = await service.GetProductInputModelAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddProductInputModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = (await service.GetProductInputModelAsync()).Categories;
                return View(model);
            }

            await service.AddProductAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int id)
        {
            await service.DeleteProductAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
