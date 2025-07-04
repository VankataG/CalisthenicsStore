
using CalisthenicsStore.ViewModels.Product;

namespace CalisthenicsStore.Services.Interfaces
{
    public interface IProductService
    {
        //READ
        Task<IEnumerable<ProductViewModel>> GetAllAsync();

        Task<IEnumerable<ProductViewModel>> GetByCategoryAsync(int categoryId);

        Task<ProductViewModel?> GetByIdAsync(int id);


        //CREATE 
        Task<ProductInputModel> GetProductInputModelAsync();

        Task AddProductAsync(ProductInputModel inputModel);

        //EDIT
        Task<ProductInputModel?> GetEditableProductAsync(int id);

        Task EditProductAsync(ProductInputModel model);

        //DELETE
        Task HardDeleteProductAsync(int id);
    }
}
