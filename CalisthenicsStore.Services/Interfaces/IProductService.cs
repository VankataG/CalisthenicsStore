using CalisthenicsStore.ViewModels.Product;

namespace CalisthenicsStore.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductViewModel>> GetAllAsync();

        Task<IEnumerable<ProductViewModel>> GetByCategoryAsync(int categoryId);

         Task<ProductViewModel?> GetByIdAsync(int id);
    }
}
