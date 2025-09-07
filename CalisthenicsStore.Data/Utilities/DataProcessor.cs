using System.Text.Json;
using Microsoft.EntityFrameworkCore;

using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Data.Utilities.Interfaces;

namespace CalisthenicsStore.Data.Utilities
{
    public class DataProcessor
    {
        private readonly IValidator entityValidator;

        public DataProcessor(IValidator entityValidator)
        {
            this.entityValidator = entityValidator;
        }

        public async Task ImportProductsFromJson(CalisthenicsStoreDbContext dbContext)
        {
            if (await dbContext.Products.AnyAsync()) return;

            string path = Path.Combine(AppContext.BaseDirectory, "Files", "products.json");
            Console.WriteLine("[SEED] Looking for: " + path);


            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Seed file not found: {path}");
            }

            string productsStr = await File.ReadAllTextAsync(path);

            var products = JsonSerializer.Deserialize<List<Product>>(productsStr);

            if (products != null && products.Count > 0)
            {
                    await dbContext.Products.AddRangeAsync(products);
                    await dbContext.SaveChangesAsync();
            }
        }

    }
}
