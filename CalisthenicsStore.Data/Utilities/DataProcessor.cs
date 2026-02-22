using System.Text.Json;
using Microsoft.EntityFrameworkCore;

using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Data.Utilities.Interfaces;
using CalisthenicsStore.Data.Utilities.DTOs;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CalisthenicsStore.Data.Utilities
{
    public class DataProcessor
    {
        private readonly IValidator entityValidator;

        public DataProcessor(IValidator entityValidator)
        {
            this.entityValidator = entityValidator;
        }

        public async Task ImportProductsFromJson(CalisthenicsStoreDbContext dbContext, string supabaseUrl, string bucket)
        {
            if (await dbContext.Products.AnyAsync()) return;

            string path = Path.Combine(AppContext.BaseDirectory, "Files", "products.json");
            Console.WriteLine("[SEED] Looking for: " + path);

            if (!File.Exists(path))
                throw new FileNotFoundException($"Seed file not found: {path}");
            
            string productsData = await File.ReadAllTextAsync(path);

            var productsDtos = JsonSerializer.Deserialize<List<SeedProductDto>>(productsData);

            if (productsDtos == null || productsDtos.Count == 0) return;


            supabaseUrl = supabaseUrl.TrimEnd('/');
            bucket = bucket.Trim('/');

            string basePublicUrl = $"{supabaseUrl}/storage/v1/object/public/{bucket}/";


            List<Product> products = new List<Product>();
            foreach (var productDto in productsDtos)
            {
                products.Add(new Product
                {
                    Name = productDto.Name,
                    Description = productDto.Description,
                    Price = productDto.Price,
                    ImageUrl = string.IsNullOrWhiteSpace(productDto.ImagePath) ? "/images/no-image.jpg" : basePublicUrl + productDto.ImagePath.TrimStart('/'),
                    CategoryId = productDto.CategoryId,
                    StockQuantity = productDto.StockQuantity
                });
            }

            await dbContext.Products.AddRangeAsync(products);
            await dbContext.SaveChangesAsync();
        }

    }
}
