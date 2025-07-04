using CalisthenicsStore.Data.Models;

namespace CalisthenicsStore.Data.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product, int>, IAsyncRepository<Product, int>
    {
        IQueryable<Product> GetAllAttackedWithCategory();
    }

}
