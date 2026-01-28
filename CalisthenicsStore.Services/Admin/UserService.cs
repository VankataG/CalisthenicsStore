using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Data.Repositories.Interfaces;
using CalisthenicsStore.Services.Admin.Interfaces;
using CalisthenicsStore.ViewModels.Admin.UserManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static CalisthenicsStore.Common.RolesConstants;



namespace CalisthenicsStore.Services.Admin
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IOrderRepository orderRepository;

        public UserService(UserManager<ApplicationUser> userManager, IOrderRepository orderRepository)
        {
            this.userManager = userManager;
            this.orderRepository = orderRepository;
        }

        public async Task<IEnumerable<UserManagementIndexViewModel>> GetUsersBoardDataAsync(string userId)
        {
            List<ApplicationUser> users = await userManager
                .Users
                .Where(u => u.Id.ToString().ToLower() != userId.ToLower())
                .AsNoTracking()
                .ToListAsync();

            var allUsers = new List<UserManagementIndexViewModel>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);

                allUsers.Add(new UserManagementIndexViewModel
                {
                    Id = user.Id.ToString(),
                    FullName = $"{user.FirstName} {user.LastName}",
                    Email = user.Email!,
                    Role = roles.FirstOrDefault() ?? UserRoleName
                });
            }

            return allUsers;
        }

        public async Task<bool> ChangeRoleAsync(Guid userId, string newRole)
        {
            ApplicationUser? user = await userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return false;

            var currentRoles = await userManager.GetRolesAsync(user);
            IdentityResult removeResult = await userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
                return false;

            IdentityResult addResult = await userManager.AddToRoleAsync(user, newRole);
            return addResult.Succeeded;
        }

        public async Task<IEnumerable<UserOrderViewModel>> GetUserOrdersAsync(Guid userId)
        {
            IEnumerable<UserOrderViewModel> userOrders = await orderRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .Where(o => o.ApplicationUserId == userId)
                .Select(o => new UserOrderViewModel()
                {
                    Id = o.Id,
                    City = o.City,
                    Address = o.Address,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    Products = o.Products
                        .Select(p => new UserOrderProductViewModel()
                        {
                            ProductName = p.Product.Name,
                            Quantity = p.Quantity
                        })
                        .ToArray()
                })
                .ToArrayAsync();

            return userOrders;
        }
    }
}
