using CalisthenicsStore.ViewModels.Admin.ProductManagement;

namespace CalisthenicsStore.Services.Admin.Interfaces
{
    public interface IProductManagementService
    {
        Task<IEnumerable<ProductManagementIndexViewModel>> GetProductBoardDataAsync();
    }
}
