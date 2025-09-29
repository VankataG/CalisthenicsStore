using Microsoft.AspNetCore.Mvc;

using CalisthenicsStore.Services.Admin.Interfaces;
using CalisthenicsStore.ViewModels.Admin.ProductManagement;
using static CalisthenicsStore.Common.Constants.Notifications;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using CalisthenicsStore.Services;

namespace CalisthenicsStore.Web.Areas.Admin.Controllers
{
    public class ProductManagementController : BaseAdminController
    {
        private readonly IProductManagementService productService;

        private readonly ILogger<ExerciseManagementController> logger;

        public ProductManagementController(IProductManagementService productService, ILogger<ExerciseManagementController> logger)
        {
            this.productService = productService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<ProductManagementIndexViewModel> allProducts = 
                await this.productService.GetProductBoardDataAsync();

            return View(allProducts);
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

            try
            {
                bool isSuccess = await productService.AddProductAsync(model);

                if (!isSuccess)
                {
                    TempData[ErrorMessageKey] = "Error occurred while adding the product!";
                }
                else
                {
                    TempData[SuccessMessageKey] = "Product added successfully!";
                }
            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "Unexpected error occured while adding the product! Please contact the developer team.";
            }

            
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                ProductInputModel? editableProduct = await productService.GetEditableProductAsync(id);

                if (editableProduct == null)
                {
                    logger.LogWarning("Attempted to edit product with ID {ProductId}, but it was not found.", id);

                    TempData[ErrorMessageKey] = "Product was not found!";

                    return RedirectToAction(nameof(Index));
                }
                
                return View(editableProduct);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error occurred while trying to edit product with ID {ProductId}", id);
                TempData[ErrorMessageKey] = "Unexpected error occured while trying to edit the product!";

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


            try
            {
                bool isSuccess = await productService.EditProductAsync(model);

                if (!isSuccess)
                {
                    TempData[ErrorMessageKey] = "Error occured while editing the product!";
                }
                else
                {
                    TempData[SuccessMessageKey] = "Product updated successfully!";
                }
            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = "Unexpected error occured while editing the exercise! Please contact the developer team.";
            }

            return RedirectToAction(nameof(Index), new { id = model.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOrRestore(Guid id)
        {
            try
            {
                Tuple<bool, string> results = await productService.DeleteOrRestoreAsync(id);

                bool isSuccess = results.Item1;
                string action = results.Item2;

                if (!isSuccess)
                {
                    TempData[ErrorMessageKey] = $"Error occured while trying to {action} product!";
                }
                else
                {
                    TempData[SuccessMessageKey] = $"The {action} was successful!";
                }

            }
            catch (Exception)
            {
                TempData[ErrorMessageKey] = $"Unexpected error occured!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
