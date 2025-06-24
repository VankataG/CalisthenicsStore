using System.Security.Claims;
using CalisthenicsStore.Data;
using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Services.Interfaces;
using CalisthenicsStore.ViewModels.CartItem;
using CalisthenicsStore.ViewModels.Order;

namespace CalisthenicsStore.Services
{
    public class OrderService : IOrderService
    {
        private readonly CalisthenicsStoreDbContext context;

        private readonly ICartService cartService;

        public OrderService(CalisthenicsStoreDbContext context, ICartService cartService)
        {
            this.context = context;
            this.cartService = cartService;
        }


        public CheckoutViewModel CheckoutCartItems()
        {
            var cartItems = cartService.GetCart();

            var model = new CheckoutViewModel
            {
                CartItems = cartItems.Select(ci => new CartItemViewModel
                {
                    ProductName = ci.Product.Name,
                    Quantity = ci.Quantity,
                    Price = ci.Product.Price
                }),
                TotalPrice = cartItems.Sum(ci => ci.Product.Price * ci.Quantity),

                CustomerName = "",
                Address = "",
                City = ""
            };

            return model;
        }

        public Task<int> PlaceOrderAsync(CheckoutViewModel model)
        {
            IEnumerable<CartItem> cartItems = cartService.GetCart();

            if (!cartItems.Any())
            {
                throw new InvalidOperationException("Cart is empty!");
            }

            Order order = new Order()
            {
                CustomerName = model.CustomerName,
                Address = model.Address,
                City = model.City,
                OrderDate = DateTime.Now,
                Email = ,
                Products = new List<OrderProduct>()
            }
        }
    }
}
