
using CalisthenicsStore.Services.Admin.Interfaces;
using CalisthenicsStore.ViewModels.Admin.OrderManagement;
using CalisthenicsStore.ViewModels.Order;
using Microsoft.AspNetCore.Mvc;
using static CalisthenicsStore.Common.Constants.Notifications;

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

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            ProfileOrderViewModel? orderModel = await orderService.GetOrderDataAsync(id);

            return View(orderModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOrRestore(Guid id)
        {
            try
            {
                Tuple<bool, string> results = await orderService.DeleteOrRestoreAsync(id);

                bool isSuccess = results.Item1;
                string action = results.Item2;

                if (!isSuccess)
                {
                    TempData[ErrorMessageKey] = $"Error occured while trying to {action} order!";
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
