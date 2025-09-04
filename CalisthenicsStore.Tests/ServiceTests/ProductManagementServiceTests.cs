using CalisthenicsStore.Data.Repositories.Interfaces;
using CalisthenicsStore.Services.Admin;
using CalisthenicsStore.Services.Admin.Interfaces;
using Moq;
using NUnit.Framework;

namespace CalisthenicsStore.Tests.ServiceTests
{

    [TestFixture]
    public class ProductManagementServiceTests
    {
        private IMock<IProductRepository> productRepositoryMock;
        private IMock<ICategoryRepository> categoryRepositoryMock;

        private IProductManagementService productService;

        [SetUp]
        public void SetUp()
        {
            this.productRepositoryMock = new Mock<IProductRepository>();
            this.categoryRepositoryMock = new Mock<ICategoryRepository>();
            this.productService =
                new ProductManagementService(productRepositoryMock.Object, categoryRepositoryMock.Object);
        }

        [Test]
        public void PassAlways()
        {
            Assert.Pass();
        }

    }
}
