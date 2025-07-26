using Microsoft.AspNetCore.Mvc;

using CalisthenicsStore.Services.Admin.Interfaces;
using CalisthenicsStore.ViewModels.Admin.UserManagement;


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

        public IActionResult ChangeUserRole()
        {
            throw new NotImplementedException();
        }
    }
}
