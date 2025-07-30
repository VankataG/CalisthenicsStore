using Microsoft.AspNetCore.Mvc;

using CalisthenicsStore.Services.Admin.Interfaces;
using CalisthenicsStore.ViewModels.Admin.ProductManagement;
using static CalisthenicsStore.Common.Constants.Notifications;

namespace CalisthenicsStore.Web.Areas.Admin.Controllers
{
    public class ProductManagementController : BaseAdminController
    {
        private readonly IProductManagementService productService;

        public ProductManagementController(IProductManagementService productService)
        {
            this.productService = productService;
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
    }
}
