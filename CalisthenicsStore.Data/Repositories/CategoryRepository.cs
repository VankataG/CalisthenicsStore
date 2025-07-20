using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Data.Repositories.Interfaces;

namespace CalisthenicsStore.Data.Repositories
{
    public class CategoryRepository : BaseRepository<Category, Guid>, ICategoryRepository
    {
        public CategoryRepository(CalisthenicsStoreDbContext dbContext) : base(dbContext)
        {
        }
    }
}
