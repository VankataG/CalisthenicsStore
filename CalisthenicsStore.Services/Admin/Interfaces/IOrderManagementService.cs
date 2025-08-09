using CalisthenicsStore.ViewModels.Admin.OrderManagement;

namespace CalisthenicsStore.Services.Admin.Interfaces
{
    public interface IOrderManagementService
    {
        Task<IEnumerable<OrderManagementIndexViewModel>> GetOrderBoardDataAsync();
    }
}
