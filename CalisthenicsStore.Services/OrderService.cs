
using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Data.Repositories.Interfaces;
using CalisthenicsStore.Services.Interfaces;
using CalisthenicsStore.ViewModels.CartItem;
using CalisthenicsStore.ViewModels.Order;
using CalisthenicsStore.ViewModels.Payment;
using Microsoft.EntityFrameworkCore;

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
                Address = "",
                City = ""
            };

            return model;
        }

        public async Task<Guid> PlaceOrderAsync(CheckoutViewModel model, string userId)
        {
            IEnumerable<CartItem> cartItems = cartService.GetCart();

            if (!cartItems.Any())
            {
                throw new InvalidOperationException("Cart is empty!");
            }

            Order order = new Order()
            {
                ApplicationUserId = Guid.Parse(userId),
                Address = model.Address,
                City = model.City,
                OrderDate = DateTime.Now,
                Status = "Pending",
                Products = new List<OrderProduct>(),
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
          
            return order.Id;
        }

        public async Task<PaymentViewModel?> GetPaymentViewModelAsync(Guid orderId)
        {
            PaymentViewModel? model = await repository
                .GetAllAttached()
                .AsNoTracking()
                .Where(o => o.Id == orderId)
                .Select(o => new PaymentViewModel()
                {
                    CartItems = o.Products
                        .Select(p => new CartItemViewModel()
                        {
                            ProductId = p.ProductId,
                            ProductName = p.Product.Name,
                            ImageUrl = p.Product.ImageUrl,
                            Price = p.UnitPrice,
                            Quantity = p.Quantity
                        })
                        .ToList(),
                    TotalPrice = o.Products.Sum(p => p.UnitPrice * p.Quantity)
                })
                .SingleOrDefaultAsync();

            return model;
        }

        public async Task MarkOrderAsPaidAsync(Guid orderId)
        {
            Order? order = await repository.GetByIdAsync(orderId);

            if (order is null) return;

            order.Status = "Paid";

            await repository.UpdateAsync(order);
            cartService.ClearCart();
        }
    }
}
