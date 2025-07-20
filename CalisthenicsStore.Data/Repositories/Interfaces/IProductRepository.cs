using CalisthenicsStore.Data.Models;

namespace CalisthenicsStore.Data.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product, Guid>, IAsyncRepository<Product, Guid>
    {
        IQueryable<Product> GetAllAttackedWithCategory();
    }

}
