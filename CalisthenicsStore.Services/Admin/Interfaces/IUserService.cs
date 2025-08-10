using CalisthenicsStore.ViewModels.Admin.UserManagement;

namespace CalisthenicsStore.Services.Admin.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserManagementIndexViewModel>> GetUsersBoardDataAsync(string userId);

        Task<bool> ChangeRoleAsync(Guid userId, string newRole);

        Task<IEnumerable<UserOrderViewModel>> GetUserOrdersAsync(Guid userId);

    }
}
