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
            string path = Path.Combine(AppContext.BaseDirectory, "Files", "products.json");
            string productsStr = await File.ReadAllTextAsync(path);

            var products = JsonSerializer.Deserialize<List<Product>>(productsStr);

            if (products != null && products.Count > 0)
            {
                List<int> productsIds = products
                    .Select(p => p.Id)
                    .ToList();

                if (await dbContext.Products.AnyAsync(p => productsIds.Contains(p.Id)) == false)
                {
                    await dbContext.Products.AddRangeAsync(products);
                    await dbContext.SaveChangesAsync();
                }
            }
        }

    }
}
