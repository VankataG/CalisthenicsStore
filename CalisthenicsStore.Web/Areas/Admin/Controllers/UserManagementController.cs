using Microsoft.AspNetCore.Mvc;

using CalisthenicsStore.Services.Admin.Interfaces;
using CalisthenicsStore.ViewModels.Admin.UserManagement;
using static CalisthenicsStore.Common.Constants.Notifications;


namespace CalisthenicsStore.Web.Areas.Admin.Controllers
{
    public class UserManagementController : BaseAdminController
    {
        private readonly IUserService userService;

        public UserManagementController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<UserManagementIndexViewModel> allUsers
                = await this.userService.GetUsersBoardDataAsync(this.GetUserId().ToLower());

            return View(allUsers);
        }

        public async Task<IActionResult> ChangeUserRole(Guid id, string newRole)
        {
            bool isSuccess = await userService.ChangeRoleAsync(id, newRole);

            if (isSuccess)
            {
                TempData[SuccessMessageKey] = "Successfully updated user role!";
            }
            else
            {
                TempData[ErrorMessageKey] = "Failed to change user role.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
