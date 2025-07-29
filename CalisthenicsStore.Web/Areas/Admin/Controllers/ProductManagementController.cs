using CalisthenicsStore.Services.Admin.Interfaces;
using CalisthenicsStore.ViewModels.Admin.ProductManagement;
using Microsoft.AspNetCore.Mvc;

namespace CalisthenicsStore.Web.Areas.Admin.Controllers
{
    public class ProductManagementController : BaseAdminController
    {
        private readonly IProductManagementService productService;

        public ProductManagementController(IProductManagementService productService)
        {
            this.productService = productService;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<ProductManagementIndexViewModel> allProducts = 
                await this.productService.GetProductBoardDataAsync();

            return View(allProducts);
        }
    }
}
