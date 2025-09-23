using CalisthenicsStore.ViewModels.Admin.OrderManagement;
using CalisthenicsStore.ViewModels.Order;

namespace CalisthenicsStore.Services.Admin.Interfaces
{
    public interface IOrderManagementService
    {
        Task<IEnumerable<OrderManagementIndexViewModel>> GetOrderBoardDataAsync();

        Task<ProfileOrderViewModel?> GetOrderDataAsync(Guid id);

        Task<Tuple<bool, string>> DeleteOrRestoreAsync(Guid id);
    }
}
