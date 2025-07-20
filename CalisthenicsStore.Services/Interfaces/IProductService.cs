
using CalisthenicsStore.ViewModels.Product;

namespace CalisthenicsStore.Services.Interfaces
{
    public interface IProductService
    {
        //READ
        Task<IEnumerable<ProductViewModel>> GetAllAsync();

        Task<IEnumerable<ProductViewModel>> GetByCategoryAsync(Guid categoryId);

        Task<ProductViewModel?> GetByIdAsync(Guid id);


        //CREATE 
        Task<ProductInputModel> GetProductInputModelAsync();

        Task AddProductAsync(ProductInputModel inputModel);

        //EDIT
        Task<ProductInputModel?> GetEditableProductAsync(Guid id);

        Task EditProductAsync(ProductInputModel model);

        //DELETE
        Task DeleteProductAsync(Guid id);
        Task HardDeleteProductAsync(Guid id);
    }
}
