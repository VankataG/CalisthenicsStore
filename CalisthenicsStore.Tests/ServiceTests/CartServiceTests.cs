using System.Text.Json;
using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Data.Repositories;
using CalisthenicsStore.Data.Repositories.Interfaces;
using CalisthenicsStore.Services;
using CalisthenicsStore.Services.Interfaces;
using CalisthenicsStore.Tests.ServiceTests.Other;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollector.InProcDataCollector;
using Moq;
using NUnit.Framework;

namespace CalisthenicsStore.Tests.ServiceTests
{
    [TestFixture]
    public class CartServiceTests
    {
        private ISession fakeSession;

        private Mock<IHttpContextAccessor> httpContextAccessorMock;

        private Mock<IProductRepository> productRepositoryMock;

        private ICartService cartService;

        [SetUp]
        public void SetUp()
        {
            DefaultHttpContext context = new DefaultHttpContext();
            fakeSession = new TestSession();
            context.Session = fakeSession;
            this.httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            this.httpContextAccessorMock
                .Setup(ca => ca.HttpContext)
                .Returns(context);

            this.productRepositoryMock = new Mock<IProductRepository>();
            this.cartService = new CartService(this.productRepositoryMock.Object, httpContextAccessorMock.Object);
        }

        [Test]
        public void PassAlways()
        {
            Assert.Pass();
        }

        [Test]
        public void GetCartShouldReturnEmptyListWithNoCartInSession()
        {
            List<CartItem> cart = cartService.GetCart();

            Assert.That(cart, Is.Not.Null);
            Assert.That(cart, Is.Empty);
        }

        [Test]
        public void GetCartShouldReturnCartItems()
        {
            List<CartItem> cartItems = new List<CartItem>()
            {
                new CartItem()
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 5
                },
                new CartItem()
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 10
                }
            };

            string cartJson = JsonSerializer.Serialize(cartItems);
            fakeSession.SetString("Cart", cartJson);

            List<CartItem> result = cartService.GetCart();

            Assert.That(result.Count, Is.EqualTo(cartItems.Count));
            Assert.That(result[0].Quantity, Is.EqualTo(5));
            Assert.That(result[1].Quantity, Is.EqualTo(10));
        }

    }
}
