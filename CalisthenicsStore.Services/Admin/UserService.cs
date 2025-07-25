using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Services.Admin.Interfaces;
using CalisthenicsStore.ViewModels.Admin.UserManagement;
using static CalisthenicsStore.Common.RolesConstants;



namespace CalisthenicsStore.Services.Admin
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IEnumerable<UserManagementIndexViewModel>> GetUsersBoardDataAsync(string userId)
        {
            IEnumerable<UserManagementIndexViewModel> allUsers = await userManager
                .Users
                .Where(u => u.Id.ToString().ToLower() != userId.ToLower())
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

            return allUsers;
        }
    }
}
