
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Services.Admin.Interfaces;
using CalisthenicsStore.ViewModels.Admin.UserManagement;
using static CalisthenicsStore.Common.RolesConstants;

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
