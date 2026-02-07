using CalisthenicsStore.Data.Repositories.Interfaces;
using CalisthenicsStore.Services.Admin;
using CalisthenicsStore.Services.Admin.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace CalisthenicsStore.Tests.ServiceTests
{

    [TestFixture]
    public class ProductManagementServiceTests
    {
        private IMock<IProductRepository> productRepositoryMock;
        private IMock<ICategoryRepository> categoryRepositoryMock;
        private IMock<ILogger<ProductManagementService>> loggerMock;
        private IMock<IFileStorageService> fileStorageMock;

        private IProductManagementService productService;

        [SetUp]
        public void SetUp()
        {
            this.productRepositoryMock = new Mock<IProductRepository>();
            this.categoryRepositoryMock = new Mock<ICategoryRepository>();
            this.loggerMock = new Mock<ILogger<ProductManagementService>>();
            this.fileStorageMock = new Mock<SupabaseStorageService>();
            this.productService =
                new ProductManagementService(productRepositoryMock.Object, categoryRepositoryMock.Object, loggerMock.Object, fileStorageMock.Object);
        }

        [Test]
        public void PassAlways()
        {
            Assert.Pass();
        }

    }
}
