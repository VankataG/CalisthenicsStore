using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Data.Repositories.Interfaces;
using CalisthenicsStore.Services;
using CalisthenicsStore.Services.Interfaces;
using CalisthenicsStore.Tests.ServiceTests.Other;
using CalisthenicsStore.ViewModels.CartItem;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using MockQueryable;
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

        [Test]
        public void SaveCartShouldSerializeAndStoreInSession()
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

            cartService.SaveCart(cartItems);

            bool result = fakeSession.TryGetValue("Cart", out var value);
            Assert.That(result, Is.True);

            string jsonCartItems = Encoding.UTF8.GetString(value);
            List<CartItem>? deserializedCartItems = JsonSerializer.Deserialize<List<CartItem>>(jsonCartItems);

            Assert.That(deserializedCartItems, Is.Not.Null);
            Assert.That(deserializedCartItems.Count, Is.EqualTo(cartItems.Count));
            Assert.That(deserializedCartItems[0].ProductId, Is.EqualTo(cartItems[0].ProductId));
            Assert.That(deserializedCartItems[1].Quantity, Is.EqualTo(cartItems[1].Quantity));
        }

        [Test]
        public async Task GetCartProductsDataAsyncShouldReturnEmptyListWithNoCartInSession()
        {
            IQueryable<Product> products = new List<Product>().BuildMock();

            productRepositoryMock
                .Setup(pr => pr.GetAllAttached())
                .Returns(products);

            cartService.SaveCart(new List<CartItem>());

            IEnumerable<CartItemViewModel> result = await cartService.GetCartProductsDataAsync();

            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetCartProductsDataAsyncShouldReturnCorrectViewModels()
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

            cartService.SaveCart(new List<CartItem>()
            {
                new CartItem()
                {
                    ProductId = product.Id,
                    Quantity = 2
                }
            });

            IEnumerable<CartItemViewModel> result = await cartService.GetCartProductsDataAsync();
            CartItemViewModel resultViewModel = result.First();
            Assert.That(result.Count(), Is.EqualTo(products.Count()));
            Assert.That(resultViewModel.ProductId, Is.EqualTo(product.Id));
            Assert.That(resultViewModel.ProductName, Is.EqualTo(product.Name));
            Assert.That(resultViewModel.Price, Is.EqualTo(product.Price));
            Assert.That(resultViewModel.Quantity, Is.EqualTo(2));
            Assert.That(resultViewModel.ImageUrl, Is.EqualTo(product.ImageUrl));
        }

        [Test]
        public async Task AddToCartAsyncShouldAddNewProduct()
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

            productRepositoryMock
                .Setup(pr => pr.FirstOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(product);

            cartService.SaveCart(new List<CartItem>());

            await cartService.AddToCartAsync(product.Id);

            List<CartItem> cart = cartService.GetCart();

            Assert.That(cart.Count(), Is.EqualTo(1));
            Assert.That(cart[0].ProductId, Is.EqualTo(product.Id));
            Assert.That(cart[0].Quantity, Is.EqualTo(1));
            Assert.That(product.StockQuantity, Is.EqualTo(4));
        }

        [Test]
        public async Task AddToCartAsyncShouldIncrementQuantityOfExistingProduct()
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

            productRepositoryMock
                .Setup(pr => pr.FirstOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(product);

            cartService.SaveCart(new List<CartItem>()
            {
                new CartItem()
                {
                    ProductId = product.Id,
                    Quantity = 1
                }
            });

            await cartService.AddToCartAsync(product.Id);

            List<CartItem> cart = cartService.GetCart();

            Assert.That(cart.Count(), Is.EqualTo(1));
            Assert.That(cart[0].ProductId, Is.EqualTo(product.Id));
            Assert.That(cart[0].Quantity, Is.EqualTo(2));
            Assert.That(product.StockQuantity, Is.EqualTo(4));
        }

        [Test]
        public void AddToCartAsyncShouldThrowWithZeroQuantity()
        {
            Product product = new Product()
            {
                Id = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                Name = "Bar",
                Price = 30,
                StockQuantity = 0,
                IsDeleted = false
            };

            productRepositoryMock
                .Setup(pr => pr.FirstOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(product);

            Assert.ThrowsAsync<Exception>( async () => await cartService.AddToCartAsync(product.Id));
        }

        [Test]
        public async Task RemoveFromCartShouldRemoveAndRestoreQuantity()
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

            productRepositoryMock
                .Setup(pr => pr.FirstOrDefaultAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(product);

            cartService.SaveCart(new List<CartItem>()
            {
                new CartItem()
                {
                    ProductId = product.Id,
                    Quantity = 2
                }
            });

            await cartService.RemoveFromCart(product.Id);

            List<CartItem> cart = cartService.GetCart();

            Assert.That(product.StockQuantity, Is.EqualTo(7));
            Assert.That(cart, Is.Empty);
        }

        [Test]
        public void ClearCartRemoveDataFromSession()
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

            cartService.SaveCart(cartItems);
            cartService.ClearCart();

            bool result = fakeSession.TryGetValue("Cart", out var value);
            Assert.That(result, Is.False);
            Assert.That(value, Is.Null);
        }
    }
}
