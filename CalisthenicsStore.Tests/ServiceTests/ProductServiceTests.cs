using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Data.Repositories.Interfaces;
using CalisthenicsStore.Services;
using CalisthenicsStore.Services.Interfaces;
using CalisthenicsStore.ViewModels.Product;
using MockQueryable;
using Moq;
using NUnit.Framework;

namespace CalisthenicsStore.Tests.ServiceTests
{
    [TestFixture]
    public class ProductServiceTests
    {
        private Mock<IProductRepository> productRepositoryMock;
        private Mock<ICategoryRepository> categoryRepositoryMock;
        private IProductService productService;

        [SetUp]
        public void SetUp()
        {
            this.productRepositoryMock = new Mock<IProductRepository>();
            this.categoryRepositoryMock = new Mock<ICategoryRepository>();
            this.productService =
                new ProductService(this.productRepositoryMock.Object, this.categoryRepositoryMock.Object);
        }

        [Test]
        public void PassAlways()
        {
            Assert.Pass();
        }


        [Test]
        public async Task GetAllAsyncShouldReturnEmptyListWhenNoProducts()
        {
            IQueryable<Product> expectedEmptyProductsList = new List<Product>().BuildMock();

            this.productRepositoryMock
                .Setup(pr => pr.GetAllAttackedWithCategory())
                .Returns(expectedEmptyProductsList);

            IEnumerable<ProductViewModel> emptyResult = await this.productService.GetAllAsync();

            Assert.That(emptyResult, Is.Not.Null);
            Assert.That(emptyResult.Count(), Is.EqualTo(expectedEmptyProductsList.Count()));
        }

        [Test]
        public async Task GetAllAsyncShouldReturnSameCollection()
        {

            Category barsCategory = new Category()
            {
                Id = Guid.Parse("5835e49a-45b7-462f-a3fe-1dd06d43dce3"),
                Name = "Bars",
                IsDeleted = false
            };
            Category ringsCategory = new Category()
            {
                Id = Guid.Parse("ea054ff5-be78-45b4-a4cb-398f8e7fb0fc"),
                Name = "Rings",
                IsDeleted = false
            };


            IQueryable<Product> expectedProductsListQueryable = new List<Product>()
                {
                    new Product()
                    {
                        Id = Guid.Parse("307d5fed-4c5f-4515-863b-dabd1c3ac4c4"),
                        CategoryId = barsCategory.Id,
                        Category = barsCategory,
                        Name = "Door Bar",
                        Price = 50.00M,
                        StockQuantity = 5,
                        IsDeleted = false
                    },
                    new Product()
                    {
                        Id = Guid.Parse("7ab46c82-7c55-4882-8bce-47028c188003"),
                        CategoryId = ringsCategory.Id,
                        Category = ringsCategory,
                        Name = "Wooden Rings",
                        Price = 30.00M,
                        StockQuantity = 3,
                        IsDeleted = false
                    }
                }
                .BuildMock();

            this.productRepositoryMock
                .Setup(pr => pr.GetAllAttackedWithCategory())
                .Returns(expectedProductsListQueryable);

            IEnumerable<ProductViewModel> emptyResult = await this.productService.GetAllAsync();

            Assert.That(emptyResult, Is.Not.Null);
            Assert.That(emptyResult.Count(), Is.EqualTo(expectedProductsListQueryable.Count()));
        }

        [Test]
        public async Task GetByCategoryAsyncShouldReturnEmptyListWithNonExistingCategory()
        {
            Category barsCategory = new Category()
            {
                Id = Guid.Parse("5835e49a-45b7-462f-a3fe-1dd06d43dce3"),
                Name = "Bars",
                IsDeleted = false
            };
            Category ringsCategory = new Category()
            {
                Id = Guid.Parse("ea054ff5-be78-45b4-a4cb-398f8e7fb0fc"),
                Name = "Rings",
                IsDeleted = false
            };


            IQueryable<Product> expectedProductsListQueryable = new List<Product>()
                {
                    new Product()
                    {
                        Id = Guid.Parse("307d5fed-4c5f-4515-863b-dabd1c3ac4c4"),
                        CategoryId = barsCategory.Id,
                        Category = barsCategory,
                        Name = "Door Bar",
                        Price = 50.00M,
                        StockQuantity = 5,
                        IsDeleted = false
                    },
                    new Product()
                    {
                        Id = Guid.Parse("7ab46c82-7c55-4882-8bce-47028c188003"),
                        CategoryId = ringsCategory.Id,
                        Category = ringsCategory,
                        Name = "Wooden Rings",
                        Price = 30.00M,
                        StockQuantity = 3,
                        IsDeleted = false
                    }
                }
                .BuildMock();

            this.productRepositoryMock
                .Setup(pr => pr.GetAllAttackedWithCategory())
                .Returns(expectedProductsListQueryable);

            IEnumerable<ProductViewModel> emptyColl =
                await this.productService.GetByCategoryAsync(Guid.Parse("e4b9cce9-e204-46a2-8a25-51b030117d12"));

            Assert.That(emptyColl, Is.Empty);
        }

        [Test]
        public async Task GetByCategoryAsyncShouldReturnMatchingFilteredResults()
        {
            Category barsCategory = new Category()
            {
                Id = Guid.Parse("5835e49a-45b7-462f-a3fe-1dd06d43dce3"),
                Name = "Bars",
                IsDeleted = false
            };
            Category ringsCategory = new Category()
            {
                Id = Guid.Parse("ea054ff5-be78-45b4-a4cb-398f8e7fb0fc"),
                Name = "Rings",
                IsDeleted = false
            };


            IQueryable<Product> expectedProductsListQueryable = new List<Product>()
                {
                    new Product()
                    {
                        Id = Guid.Parse("307d5fed-4c5f-4515-863b-dabd1c3ac4c4"),
                        CategoryId = barsCategory.Id,
                        Category = barsCategory,
                        Name = "Door Bar",
                        Price = 50.00M,
                        StockQuantity = 5,
                        IsDeleted = false
                    },
                    new Product()
                    {
                        Id = Guid.Parse("5e0aa616-8d68-49fd-9884-d73d39f8b60c"),
                        CategoryId = barsCategory.Id,
                        Category = barsCategory,
                        Name = "Pull-Up Station",
                        Price = 200.00M,
                        StockQuantity = 2,
                        IsDeleted = false
                    },
                    new Product()
                    {
                        Id = Guid.Parse("7ab46c82-7c55-4882-8bce-47028c188003"),
                        CategoryId = ringsCategory.Id,
                        Category = ringsCategory,
                        Name = "Wooden Rings",
                        Price = 30.00M,
                        StockQuantity = 3,
                        IsDeleted = false
                    }
                }
                .BuildMock();

            this.productRepositoryMock
                .Setup(pr => pr.GetAllAttackedWithCategory())
                .Returns(expectedProductsListQueryable);

            IEnumerable<ProductViewModel> returnedProductsVm =
                await this.productService.GetByCategoryAsync(barsCategory.Id);

            foreach (ProductViewModel productVm in returnedProductsVm)
            {
                Assert.That(productVm.CategoryName, Is.EqualTo(barsCategory.Name));
            }

            Assert.That(returnedProductsVm.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetByIdAsyncShouldReturnCorrectProduct()
        {
            Category barsCategory = new Category()
            {
                Id = Guid.Parse("5835e49a-45b7-462f-a3fe-1dd06d43dce3"),
                Name = "Bars",
                IsDeleted = false
            };
            Category ringsCategory = new Category()
            {
                Id = Guid.Parse("ea054ff5-be78-45b4-a4cb-398f8e7fb0fc"),
                Name = "Rings",
                IsDeleted = false
            };

            Product searchedProduct = new Product()
            {
                Id = Guid.Parse("5e0aa616-8d68-49fd-9884-d73d39f8b60c"),
                CategoryId = barsCategory.Id,
                Category = barsCategory,
                Name = "Pull-Up Station",
                Price = 200.00M,
                StockQuantity = 2,
                IsDeleted = false
            };

            IQueryable<Product> expectedProductsListQueryable = new List<Product>()
                {
                    new Product()
                    {
                        Id = Guid.Parse("307d5fed-4c5f-4515-863b-dabd1c3ac4c4"),
                        CategoryId = barsCategory.Id,
                        Category = barsCategory,
                        Name = "Door Bar",
                        Price = 50.00M,
                        StockQuantity = 5,
                        IsDeleted = false
                    },
                    new Product()
                    {
                        Id = Guid.Parse("7ab46c82-7c55-4882-8bce-47028c188003"),
                        CategoryId = ringsCategory.Id,
                        Category = ringsCategory,
                        Name = "Wooden Rings",
                        Price = 30.00M,
                        StockQuantity = 3,
                        IsDeleted = false
                    },
                    searchedProduct
                }
                .BuildMock();

            this.productRepositoryMock
                .Setup(pr => pr.GetAllAttackedWithCategory())
                .Returns(expectedProductsListQueryable);

            ProductViewModel? foundProduct = await this.productService.GetByIdAsync(searchedProduct.Id);

            Assert.That(foundProduct, Is.Not.Null);
            Assert.That(foundProduct!.Id, Is.EqualTo(searchedProduct.Id));
            Assert.That(foundProduct.Name, Is.EqualTo(searchedProduct.Name));
            Assert.That(foundProduct.Description, Is.EqualTo(searchedProduct.Description));
            Assert.That(foundProduct.Price, Is.EqualTo(searchedProduct.Price));
            Assert.That(foundProduct.ImageUrl, Is.EqualTo(searchedProduct.ImageUrl));
            Assert.That(foundProduct.CategoryName, Is.EqualTo(searchedProduct.Category.Name));
        }
    }
}
