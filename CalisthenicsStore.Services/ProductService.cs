using Microsoft.EntityFrameworkCore;

using CalisthenicsStore.Data;
using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Services.Interfaces;
using CalisthenicsStore.ViewModels.Product;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;


namespace CalisthenicsStore.Services
{
    public class ProductService(CalisthenicsStoreDbContext context) : IProductService
    {
        public async Task<IEnumerable<ProductViewModel>> GetAllAsync()
        { 
            return await context
                .Products
                .Include(p => p.Category)
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

        public async Task<IEnumerable<ProductViewModel>> GetByCategoryAsync(int categoryId)
        {
            return await context
                .Products
                .Include(p => p.Category)
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

        public async Task<ProductViewModel?> GetByIdAsync(int id)
        {
            return await context
                .Products
                .Include(p => p.Category)
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

        public async Task<AddProductInputModel> GetProductInputModelAsync()
        {
            var categories = await context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToListAsync();

            AddProductInputModel model = new AddProductInputModel()
            {
                Categories = categories
            };

            return model;
        }

        public async Task AddProductAsync(AddProductInputModel inputModel)
        {
            Product newProduct = new Product
            {
                Name = inputModel.Name,
                Description = inputModel.Description,
                Price = inputModel.Price,
                StockQuantity = inputModel.StockQuantity,
                ImageUrl = inputModel.ImageUrl ?? "/images/no-image.jpg",
                CategoryId = inputModel.CategoryId,
            };

            await context.Products.AddAsync(newProduct);
            await context.SaveChangesAsync();
        }

        public Task EditProductAsync()
        {
            throw new NotImplementedException();
        }

        public async Task DeleteProductAsync(int id)
        {
            try
            {
                Product? product = await context
                    .Products
                    .SingleOrDefaultAsync(p => p.Id == id);

                if (product != null)
                {
                    context.Products.Remove(product);
                    await context.SaveChangesAsync();
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
