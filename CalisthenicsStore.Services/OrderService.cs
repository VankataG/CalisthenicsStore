using System.Security.Claims;

using Microsoft.Identity.Client;

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


        public async Task<CheckoutViewModel> CheckoutCartItemsAsync()
        {
            var cartItems = await cartService.GetCartProductsDataAsync();
   

            var model = new CheckoutViewModel
            {
                
                CartItems = cartItems,
                TotalPrice = cartItems.Sum(ci => ci.Price * ci.Quantity),
                CustomerName = "",
                Address = "",
                City = ""
            };

            return model;
        }

        public async Task<int> PlaceOrderAsync(CheckoutViewModel model, string email)
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
                Email = email,
                Status = "Pending",
                Products = new List<OrderProduct>()
            };

            foreach (CartItem cartItem in cartItems)
            {
                Product? product = await context
                    .Products
                    .FindAsync(cartItem.ProductId);

                if (product != null)
                {

                    OrderProduct orderProduct = new OrderProduct()
                    {
                        ProductId = product.Id,
                        Quantity = cartItem.Quantity,
                        UnitPrice = product.Price
                    };

                    order.Products.Add(orderProduct);

                }

            }

            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();

            cartService.ClearCart();


            return order.Id;
        }
    }
}
