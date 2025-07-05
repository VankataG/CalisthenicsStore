using Microsoft.AspNetCore.Mvc;

using CalisthenicsStore.Services.Interfaces;
using CalisthenicsStore.ViewModels.Product;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<IActionResult> Category(int id)
        {
            var products = await productService.GetByCategoryAsync(id);

            return View("Index", products); // Reuse Index view
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var product = await productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
             
            return View(product);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ProductInputModel model = await productService.GetProductInputModelAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductInputModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = (await productService.GetProductInputModelAsync()).Categories;
                return View(model);
            }

            await productService.AddProductAsync(model);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                ProductInputModel? editableProduct = await productService.GetEditableProductAsync(id);

                if (editableProduct == null)
                {
                    //TODO: Add ILogger
                    //logger.LogWarning("Attempted to edit product with ID {ProductId}, but it was not found.", id);

                    return RedirectToAction(nameof(Index));
                }
                else
                {


                    return View(editableProduct);
                }
            }
            catch (Exception e)
            {
                //TODO: Add ILogger
                //logger.LogError(ex, "Error occurred while trying to edit product with ID {ProductId}", id);

                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductInputModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await productService.EditProductAsync(model);

            return RedirectToAction(nameof(Details), new { id = model.Id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int id)
        {
            await productService.DeleteProductAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
