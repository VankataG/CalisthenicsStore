using CalisthenicsStore.ViewModels.Admin.ProductManagement;

namespace CalisthenicsStore.Services.Admin.Interfaces
{
    public interface IProductManagementService
    {
        Task<IEnumerable<ProductManagementIndexViewModel>> GetProductBoardDataAsync();

        Task<ProductInputModel> GetProductInputModelAsync();

        Task<bool> AddProductAsync(ProductInputModel model);

        Task<ProductInputModel?> GetEditableProductAsync(Guid id);

        Task<bool> EditProductAsync(ProductInputModel model);
    }
}
