using CalisthenicsStore.Data.Repositories;
using CalisthenicsStore.Data.Repositories.Interfaces;
using CalisthenicsStore.Services.Admin.Interfaces;
using CalisthenicsStore.ViewModels.Admin.OrderManagement;
using Microsoft.EntityFrameworkCore;

namespace CalisthenicsStore.Services.Admin
{
    public class OrderManagementService : IOrderManagementService
    {
        private readonly IOrderRepository orderRepository;

        public OrderManagementService(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task<IEnumerable<OrderManagementIndexViewModel>> GetOrderBoardDataAsync()
        {
            IEnumerable<OrderManagementIndexViewModel> orders = await orderRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Select(o => new OrderManagementIndexViewModel()
                {
                    Id = o.Id,
                    CustomerName = o.ApplicationUser.FirstName + " " + o.ApplicationUser.LastName,
                    City = o.City,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    IsDeleted = o.IsDeleted,
                    Address = o.Address

                })
                .ToArrayAsync();
            return orders;
        }
    }
}
