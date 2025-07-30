
using CalisthenicsStore.ViewModels.Admin.ProductManagement;
using CalisthenicsStore.ViewModels.Product;

namespace CalisthenicsStore.Services.Interfaces
{
    public interface IProductService
    {
        //READ
        Task<IEnumerable<ProductViewModel>> GetAllAsync();

        Task<IEnumerable<ProductViewModel>> GetByCategoryAsync(Guid categoryId);

        Task<ProductViewModel?> GetByIdAsync(Guid id);


        //EDIT
        Task<ProductInputModel?> GetEditableProductAsync(Guid id);

        Task EditProductAsync(ProductInputModel model);

        //DELETE
        Task DeleteProductAsync(Guid id);
        Task HardDeleteProductAsync(Guid id);
    }
}
