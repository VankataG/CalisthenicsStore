
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
        public async Task<IActionResult> Index()
        {
            List<ApplicationUser> users = await userManager.Users.ToListAsync();

            var model = users.Select(u => new UserManagementIndexViewModel()
            {
                Id = u.Id.ToString(),
                FullName = $"{u.FirstName} {u.LastName}",
                Email = u.Email,
                Role = userManager.GetRolesAsync(u).Result.FirstOrDefault() ?? UserRoleName
            });

            return View(model);
        }
    }
}
