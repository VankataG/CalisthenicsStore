using CalisthenicsStore.Data.Repositories.Interfaces;
using CalisthenicsStore.Services;
using CalisthenicsStore.Services.Interfaces;
using CalisthenicsStore.Tests.ServiceTests.Other;
using Microsoft.AspNetCore.Http;
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

        [SetUp]
        public void SetUp()
        {
            this.productRepositoryMock = new Mock<IProductRepository>();
            this.orderRepositoryMock = new Mock<IOrderRepository>();

            DefaultHttpContext context = new DefaultHttpContext();
            TestSession fakeSession = new TestSession();
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


    }
}
