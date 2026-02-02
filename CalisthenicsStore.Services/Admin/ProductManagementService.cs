using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Data.Repositories;
using CalisthenicsStore.Data.Repositories.Interfaces;
using CalisthenicsStore.Services.Admin.Interfaces;
using CalisthenicsStore.ViewModels.Admin.ProductManagement;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CalisthenicsStore.Services.Admin
{
    public class ProductManagementService : IProductManagementService
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;

        private readonly IFileStorageService supabaseService;

        private readonly ILogger<ProductManagementService> logger;

        public ProductManagementService(IProductRepository productRepository, 
            ICategoryRepository categoryRepository, 
            ILogger<ProductManagementService> logger,
            IFileStorageService supabaseService)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.logger = logger;
            this.supabaseService = supabaseService;
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
                    CategoryName = p.Category!.Name ,
                })
                .ToArrayAsync();

            return allProducts;
        }

        public async Task<ProductInputModel> GetProductInputModelAsync()
        {
            var categories = await categoryRepository
                .GetAllAttached()
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

        public async Task<bool> AddProductAsync(ProductInputModel inputModel)
        {
            string imageUrl = "/images/no-image.jpg";

            if (inputModel.ImageFile != null)
            {
                string? uploadedUrl = await supabaseService.UploadImageAsync(inputModel.ImageFile);
                
                if (!string.IsNullOrWhiteSpace(uploadedUrl))
                {
                    imageUrl = uploadedUrl;
                }
            }

            Product newProduct = new Product
            {
                Name = inputModel.Name,
                Description = inputModel.Description,
                Price = inputModel.Price,
                StockQuantity = inputModel.StockQuantity,
                ImageUrl = imageUrl,
                CategoryId = inputModel.CategoryId,
            };

            return await productRepository.AddAsync(newProduct);
        }

        public async Task<ProductEditModel?> GetEditableProductAsync(Guid id)
        {
            ProductEditModel? editableProduct = await productRepository
                .GetAllAttackedWithCategory()
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProductEditModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    ExistingImageUrl = p.ImageUrl!,
                    StockQuantity = p.StockQuantity,
                    CategoryId = p.CategoryId
                })
                .SingleOrDefaultAsync();

            if (editableProduct != null)
            {
                var categories = await categoryRepository
                    .GetAllAttached()
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

        public async Task<bool> EditProductAsync(ProductEditModel model)
        {
            bool result = false;

            Product? editableProduct = await productRepository
                .GetAllAttackedWithCategory()
                .SingleOrDefaultAsync(p => p.Id == model.Id);

            if (editableProduct != null)
            {
                editableProduct.Name = model.Name;
                editableProduct.Description = model.Description;
                editableProduct.Price = model.Price;
                editableProduct.StockQuantity = model.StockQuantity;
                editableProduct.CategoryId = model.CategoryId;

                if (model.NewImageFile != null && model.NewImageFile.Length > 0)
                {
                    string? newUrl = await supabaseService.UploadImageAsync(model.NewImageFile);
                    if (newUrl != null) 
                        editableProduct.ImageUrl = newUrl;
                }

                result = await productRepository.UpdateAsync(editableProduct);
            }

            return result;
        }

        public async Task<Tuple<bool, string>> DeleteOrRestoreAsync(Guid id)
        {
            bool result = false;
            string action = string.Empty;
            Product? product = await productRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .SingleOrDefaultAsync(p => p.Id == id);

            if (product != null)
            {
                action = product.IsDeleted ? "restore" : "delete";

                product.IsDeleted = !product.IsDeleted;
                await productRepository.SaveChangesAsync();
                result = true;
            }

            return new Tuple<bool, string>(result, action);
        }
        public async Task HardDeleteProductAsync(Guid id)
        {
            try
            {
                Product? product = await productRepository
                    .SingleOrDefaultAsync(p => p.Id == id);

                if (product != null)
                {
                    await productRepository.HardDeleteAsync(product);
                }
                else
                {
                    logger.LogWarning("Attempted to delete product with ID {ProductId}, but it was not found.", id);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while trying to delete product with ID {ProductId}", id);
            }

        }

    }
}
