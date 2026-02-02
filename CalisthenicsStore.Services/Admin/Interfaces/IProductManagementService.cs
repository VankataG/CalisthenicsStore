using CalisthenicsStore.ViewModels.Admin.ProductManagement;

namespace CalisthenicsStore.Services.Admin.Interfaces
{
    public interface IProductManagementService
    {
        Task<IEnumerable<ProductManagementIndexViewModel>> GetProductBoardDataAsync();

        Task<ProductInputModel> GetProductInputModelAsync();

        Task<bool> AddProductAsync(ProductInputModel model);

        Task<ProductEditModel?> GetEditableProductAsync(Guid id);

        Task<bool> EditProductAsync(ProductEditModel model);

        Task<Tuple<bool, string>> DeleteOrRestoreAsync(Guid id);

        Task HardDeleteProductAsync(Guid id);
    }
}
