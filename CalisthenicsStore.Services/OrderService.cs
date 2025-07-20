
using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Data.Repositories.Interfaces;
using CalisthenicsStore.Services.Interfaces;
using CalisthenicsStore.ViewModels.Order;

namespace CalisthenicsStore.Services
{
    public class OrderService : IOrderService
    {
        private readonly IProductRepository productRepository;
        private readonly IOrderRepository repository;

        private readonly ICartService cartService;

        public OrderService(IProductRepository productRepository, IOrderRepository repository, ICartService cartService)
        {
            this.productRepository = productRepository;
            this.repository = repository;
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

        public async Task<Guid> PlaceOrderAsync(CheckoutViewModel model, string email)
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
                Product? product = await productRepository
                    .GetByIdAsync(cartItem.ProductId);

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

            await repository.AddAsync(order);

            cartService.ClearCart();


            return order.Id;
        }
    }
}
