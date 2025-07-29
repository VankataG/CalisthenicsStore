using CalisthenicsStore.Data.Repositories.Interfaces;
using CalisthenicsStore.Services.Admin.Interfaces;
using CalisthenicsStore.ViewModels.Admin.ProductManagement;
using Microsoft.EntityFrameworkCore;

namespace CalisthenicsStore.Services.Admin
{
    public class ProductManagementService : IProductManagementService
    {
        private readonly IProductRepository productRepository;

        public ProductManagementService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }


        public async Task<IEnumerable<ProductManagementIndexViewModel>> GetProductBoardDataAsync()
        {
            IEnumerable<ProductManagementIndexViewModel> allProducts = await productRepository
                .GetAllAttackedWithCategory()
                .IgnoreQueryFilters()
                .Select(p => new ProductManagementIndexViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    IsDeleted = p.IsDeleted,
                    CategoryName = p.Category.Name ,
                })
                .ToArrayAsync();

            return allProducts;
        }
    }
}
