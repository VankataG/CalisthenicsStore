using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Data.Repositories.Interfaces;

namespace CalisthenicsStore.Data.Repositories
{
    public class OrderRepository : BaseRepository<Order, Guid>, IOrderRepository
    {
        public OrderRepository(CalisthenicsStoreDbContext dbContext) : base(dbContext)
        {
        }
    }
}
