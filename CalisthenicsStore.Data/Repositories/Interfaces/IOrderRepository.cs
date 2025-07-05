using CalisthenicsStore.Data.Models;

namespace CalisthenicsStore.Data.Repositories.Interfaces
{
    public interface IOrderRepository 
        : IRepository<Order, int>, IAsyncRepository<Order, int>
    {

    }
}
