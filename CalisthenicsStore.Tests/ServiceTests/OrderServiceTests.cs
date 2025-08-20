using System.Linq.Expressions;
using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Data.Repositories.Interfaces;
using CalisthenicsStore.Services;
using CalisthenicsStore.Services.Interfaces;
using CalisthenicsStore.Tests.ServiceTests.Other;
using CalisthenicsStore.ViewModels.CartItem;
using CalisthenicsStore.ViewModels.Order;
using Microsoft.AspNetCore.Http;
using MockQueryable;
using Moq;
using NUnit.Framework;

namespace CalisthenicsStore.Tests.ServiceTests
{
    [TestFixture]
    public class OrderServiceTests
    {
        private Mock<IOrderRepository> orderRepositoryMock;

        private Mock<IProductRepository> productRepositoryMock;

        private ICartService cartService;
        private IOrderService orderService;

        private TestSession fakeSession;

        [SetUp]
        public void SetUp()
        {
            this.productRepositoryMock = new Mock<IProductRepository>();
            this.orderRepositoryMock = new Mock<IOrderRepository>();

            DefaultHttpContext context = new DefaultHttpContext();
            fakeSession = new TestSession();
            context.Session = fakeSession;
            HttpContextAccessor httpContextAccessor = new HttpContextAccessor
            {
                HttpContext = context
            };

            this.cartService = new CartService(this.productRepositoryMock.Object, httpContextAccessor);

            this.orderService = new OrderService(productRepositoryMock.Object, orderRepositoryMock.Object, cartService);
        }

        [Test]
        public void PassAlways()
        {
            Assert.Pass();
        }

        [Test]
        public async Task CheckoutCartItemsAsyncShouldReturnCorrectCheckoutViewModel()
        {
            Product product = new Product()
            {
                Id = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                Name = "Bar",
                Price = 30,
                StockQuantity = 5,
                IsDeleted = false
            };

            IQueryable<Product> products = new List<Product>() { product }.BuildMock();

            productRepositoryMock
                .Setup(pr => pr.GetAllAttached())
                .Returns(products);
            productRepositoryMock
                .Setup(pr => pr.FirstOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(product);

            await cartService.AddToCartAsync(product.Id);
            await cartService.AddToCartAsync(product.Id);

            CheckoutViewModel result = await orderService.CheckoutCartItemsAsync();

            Assert.That(result.CartItems.Count(), Is.EqualTo(1));
            Assert.That(result.TotalPrice, Is.EqualTo(product.Price * 2));
            Assert.That(result.City, Is.EqualTo(""));
            Assert.That(result.Address, Is.EqualTo(""));
            Assert.That(result.CartItems.First().ProductId, Is.EqualTo(product.Id));
            Assert.That(result.CartItems.First().ProductName, Is.EqualTo(product.Name));
        }

        [Test]
        public async Task PlaceOrderAsyncShouldThrowIfCartIsEmpty()
        {
            IQueryable<Product> products = new List<Product>().BuildMock();

            productRepositoryMock
                .Setup(pr => pr.GetAllAttached())
                .Returns(products);

            CheckoutViewModel checkoutViewModel = await orderService.CheckoutCartItemsAsync();

            Assert.ThrowsAsync<InvalidOperationException>(async () => await orderService.PlaceOrderAsync(checkoutViewModel, Guid.NewGuid().ToString()));
        }

        [Test]
        public async Task PlaceOrderAsyncShouldWorkCorrectly()
        {
            Product product = new Product()
            {
                Id = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                Name = "Bar",
                Price = 30,
                StockQuantity = 5,
                IsDeleted = false
            };

            IQueryable<Product> products = new List<Product>() { product }.BuildMock();

            productRepositoryMock
                .Setup(pr => pr.GetAllAttached())
                .Returns(products);
            productRepositoryMock
                .Setup(pr => pr.FirstOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(product);
            productRepositoryMock
                .Setup(pr => pr.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(product);

            await cartService.AddToCartAsync(product.Id);
            await cartService.AddToCartAsync(product.Id);

            CheckoutViewModel model = await orderService.CheckoutCartItemsAsync();

            Guid result = await orderService.PlaceOrderAsync(model, Guid.NewGuid().ToString());

            orderRepositoryMock.Verify(or => or.AddAsync(It.Is<Order>(o =>
                o.Id == result &&
                o.Status == "Pending" &&
                o.Products.Count == 1 &&
                o.Products.First().ProductId == product.Id &&
                o.Products.First().Quantity == 2
                )), Times.Once);
        }
    }
}
