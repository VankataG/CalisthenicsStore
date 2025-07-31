using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Data.Repositories.Interfaces;
using CalisthenicsStore.Services.Interfaces;
using CalisthenicsStore.ViewModels.Product;
using CalisthenicsStore.ViewModels.Admin.ProductManagement;

namespace CalisthenicsStore.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository repository;
        private readonly ICategoryRepository categoryRepository;

        public ProductService(IProductRepository repository, ICategoryRepository categoryRepository)
        {
            this.repository = repository;
            this.categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<ProductViewModel>> GetAllAsync()
        { 
            return await repository
                .GetAllAttackedWithCategory()
                .AsNoTracking()
                .Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    CategoryName = p.Category.Name,
                    ImageUrl = p.ImageUrl
                })
                .ToListAsync();

        }

        public async Task<IEnumerable<ProductViewModel>> GetByCategoryAsync(Guid categoryId)
        {
            return await repository
                .GetAllAttackedWithCategory()
                .AsNoTracking()
                .Where(p => p.Category.Id == categoryId)
                .Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    CategoryName = p.Category.Name,
                    ImageUrl = p.ImageUrl
                })
                .ToListAsync();
        }

        public async Task<ProductViewModel?> GetByIdAsync(Guid id)
        {
            return await repository
                .GetAllAttackedWithCategory()
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    CategoryName = p.Category.Name,
                    ImageUrl = p.ImageUrl
                })
                .FirstAsync();
        }


        //DELETE
        public async Task DeleteProductAsync(Guid id)
        {
            try
            {
                Product? product = await repository
                    .SingleOrDefaultAsync(p => p.Id == id);

                if (product != null)
                {
                    await repository.DeleteAsync(product);
                }
                else
                {
                    //TODO: Add ILogger
                    //logger.LogWarning("Attempted to delete product with ID {ProductId}, but it was not found.", id);
                }
            }
            catch (Exception ex)
            {
                //TODO: Add ILogger
                //logger.LogError(ex, "Error occurred while trying to delete product with ID {ProductId}", id);

            }
        }
        public async Task HardDeleteProductAsync(Guid id)
        {
            try
            {
                Product? product = await repository
                    .SingleOrDefaultAsync(p => p.Id == id);

                if (product != null)
                {
                    await repository.HardDeleteAsync(product);
                }
                else
                {
                    //TODO: Add ILogger
                    //logger.LogWarning("Attempted to delete product with ID {ProductId}, but it was not found.", id);
                }
            }
            catch (Exception ex)
            {
                //TODO: Add ILogger
                //logger.LogError(ex, "Error occurred while trying to delete product with ID {ProductId}", id);

            }

        }
    }
}
