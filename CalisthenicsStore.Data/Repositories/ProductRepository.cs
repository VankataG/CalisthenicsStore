using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CalisthenicsStore.Data.Repositories
{
    public class ProductRepository : BaseRepository<Product, int>, IProductRepository
    {
        public ProductRepository(CalisthenicsStoreDbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<Product> GetAllAttackedWithCategory()
        {
            return dbSet
                .Include(p => p.Category)
                .AsQueryable();
        }
    }
}
