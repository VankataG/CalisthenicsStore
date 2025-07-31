using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Data.Repositories;
using CalisthenicsStore.Data.Repositories.Interfaces;
using CalisthenicsStore.Services.Admin.Interfaces;
using CalisthenicsStore.ViewModels.Admin.ProductManagement;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CalisthenicsStore.Services.Admin
{
    public class ProductManagementService : IProductManagementService
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;

        public ProductManagementService(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
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
            Product newProduct = new Product
            {
                Name = inputModel.Name,
                Description = inputModel.Description,
                Price = inputModel.Price,
                StockQuantity = inputModel.StockQuantity,
                ImageUrl = inputModel.ImageUrl ?? "/images/no-image.jpg",
                CategoryId = inputModel.CategoryId,
            };

            return await productRepository.AddAsync(newProduct);
        }

        public async Task<ProductInputModel?> GetEditableProductAsync(Guid id)
        {
            ProductInputModel? editableProduct = await productRepository
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

        public async Task<bool> EditProductAsync(ProductInputModel model)
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
                editableProduct.ImageUrl = model.ImageUrl ?? "/images/no-image.jpg";
                editableProduct.CategoryId = model.CategoryId;

                result = await productRepository.UpdateAsync(editableProduct);

            }

            return result;
        }
    }
}
