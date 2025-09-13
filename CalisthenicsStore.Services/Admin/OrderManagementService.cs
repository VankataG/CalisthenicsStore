using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Data.Repositories;
using CalisthenicsStore.Data.Repositories.Interfaces;
using CalisthenicsStore.Services.Admin.Interfaces;
using CalisthenicsStore.ViewModels.Admin.OrderManagement;
using CalisthenicsStore.ViewModels.Order;
using CalisthenicsStore.ViewModels.Product;
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

        public async Task<ProfileOrderViewModel> GetOrderDataAsync(Guid id)
        {
            Order? order = await this.orderRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .SingleOrDefaultAsync(o => o.Id == id);

            ProfileOrderViewModel? orderModel = null;

            if (order != null)
            {
                orderModel = new ProfileOrderViewModel()
                {
                    Id = order.Id,
                    ApplicationUserId = order.ApplicationUserId,
                    OrderDate = order.OrderDate,
                    Status = order.Status,
                    City = order.City,
                    Address = order.Address,
                    Products = order.Products
                        .Select(op => new ProfileProductViewModel()
                        {
                            ProductId = op.ProductId,
                            Name = op.Product.Name,
                            ImageUrl = op.Product.ImageUrl,
                            Price = op.UnitPrice,
                            Quantity = op.Quantity,
                            Total = op.UnitPrice * op.Quantity
                        })
                        .ToList()
                };
            }
            return orderModel;
        }

        public async Task<Tuple<bool, string>> DeleteOrRestoreAsync(Guid id)
        {
            bool result = false;
            string action = string.Empty;
            Order? order = await orderRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .SingleOrDefaultAsync(p => p.Id == id);

            if (order != null)
            {
                action = order.IsDeleted ? "restore" : "delete";

                order.IsDeleted = !order.IsDeleted;
                await orderRepository.SaveChangesAsync();
                result = true;
            }

            return new Tuple<bool, string>(result, action);
        }
    }
}
