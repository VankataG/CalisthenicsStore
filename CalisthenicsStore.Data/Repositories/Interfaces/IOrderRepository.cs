using CalisthenicsStore.Data.Models;

namespace CalisthenicsStore.Data.Repositories.Interfaces
{
    public interface IOrderRepository 
        : IRepository<Order, Guid>, IAsyncRepository<Order, Guid>
    {

    }
}
