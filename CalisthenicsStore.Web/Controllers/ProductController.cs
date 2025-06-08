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
            ProductInputModel model = await service.GetProductInputModelAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductInputModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = (await service.GetProductInputModelAsync()).Categories;
                return View(model);
            }

            await service.AddProductAsync(model);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                ProductInputModel? editableProduct = await service.GetEditableProductAsync(id);

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
            await service.EditProductAsync(model);

            return RedirectToAction(nameof(Details), new { id = model.Id});
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
