using CalisthenicsStore.ViewModels.Admin.UserManagement;

namespace CalisthenicsStore.Services.Admin.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserManagementIndexViewModel>> GetUsersBoardDataAsync(string userId);
    }
}
