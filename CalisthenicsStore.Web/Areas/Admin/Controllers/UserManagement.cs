
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using CalisthenicsStore.Data.Models;
using CalisthenicsStore.ViewModels.Admin.UserManagement;
using static CalisthenicsStore.Common.RolesConstants;

namespace CalisthenicsStore.Web.Areas.Admin.Controllers
{
    public class UserManagement : BaseAdminController
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserManagement(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<UserManagementIndexViewModel> model = await userManager
                .Users
                .Where(u => u.Id.ToString().ToLower() != this.GetUserId().ToLower())
                .Select(u => new UserManagementIndexViewModel()
                {
                    Id = u.Id.ToString(),
                    FullName = $"{u.FirstName} {u.LastName}",
                    Email = u.Email,
                    Role = userManager.GetRolesAsync(u).
                    GetAwaiter()
                    .GetResult()
                    .FirstOrDefault() ?? UserRoleName
                })
                .ToArrayAsync();

            return View(model);
        }

        public IActionResult ChangeUserRole()
        {
            throw new NotImplementedException();
        }
    }
}
