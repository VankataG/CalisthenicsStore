using Microsoft.EntityFrameworkCore;

using CalisthenicsStore.Data;
using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Data.Repositories.Interfaces;
using CalisthenicsStore.Services.Interfaces;
using CalisthenicsStore.ViewModels.Product;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;


namespace CalisthenicsStore.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository repository;
        private readonly CalisthenicsStoreDbContext context;

        public ProductService(IProductRepository repository, CalisthenicsStoreDbContext context)
        {
            this.repository = repository;
            this.context = context;
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

        public async Task<IEnumerable<ProductViewModel>> GetByCategoryAsync(int categoryId)
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

        public async Task<ProductViewModel?> GetByIdAsync(int id)
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


        //CREATE
        public async Task<ProductInputModel> GetProductInputModelAsync()
        {
            var categories = await context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToListAsync();

            ProductInputModel model = new ProductInputModel()
            {
                Categories = categories
            };

            return model;
        }

        public async Task AddProductAsync(ProductInputModel inputModel)
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

            await repository.AddAsync(newProduct);
        }

        //EDIT
        public async Task<ProductInputModel?> GetEditableProductAsync(int id)
        {
         


            ProductInputModel?  editableProduct = await repository
                .GetAllAttackedWithCategory()
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProductInputModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    ImageUrl = p.ImageUrl,
                    CategoryId = p.CategoryId
                })
                .SingleOrDefaultAsync();

            if (editableProduct != null)
            {
                var categories = await context.Categories
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    })
                    .ToListAsync();

                editableProduct.Categories = categories;
            }

            return editableProduct;
        }

        public async Task EditProductAsync(ProductInputModel model)
        {
            Product? editableProduct = await repository
                .GetAllAttackedWithCategory()
                .SingleOrDefaultAsync(p => p.Id == model.Id);

            if (editableProduct != null)
            {
                editableProduct.Name = model.Name;
                editableProduct.Description = model.Description;
                editableProduct.Price = model.Price;
                editableProduct.StockQuantity = model.StockQuantity;
                editableProduct.ImageUrl = model.ImageUrl ?? "/images/no-image.jpg";
                editableProduct.CategoryId = model.CategoryId;

                await context.SaveChangesAsync();

            }
                
        }


        //DELETE
        public async Task HardDeleteProductAsync(int id)
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
