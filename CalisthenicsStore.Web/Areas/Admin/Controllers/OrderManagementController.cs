using CalisthenicsStore.Services.Admin.Interfaces;
using CalisthenicsStore.ViewModels.Admin.OrderManagement;
using Microsoft.AspNetCore.Mvc;

namespace CalisthenicsStore.Web.Areas.Admin.Controllers
{
    public class OrderManagementController : BaseAdminController
    {
        private readonly IOrderManagementService orderService;

        public OrderManagementController(IOrderManagementService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<OrderManagementIndexViewModel> orders = await orderService.GetOrderBoardDataAsync();
            return View(orders);
        }
    }
}
