using Microsoft.AspNetCore.Mvc;

using CalisthenicsStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using CalisthenicsStore.ViewModels.Admin.ProductManagement;

namespace CalisthenicsStore.Web.Controllers
{
    public class ProductController(IProductService productService) : BaseController
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var products = await productService.GetAllAsync();

            return View(products);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Category(Guid id)
        {
            var products = await productService.GetByCategoryAsync(id);

            return View("Index", products); // Reuse Index view
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid id)
        {
            var product = await productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
             
            return View(product);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(Guid id)
        {
            await productService.DeleteProductAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
