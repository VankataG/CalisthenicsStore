

using CalisthenicsStore.Data.Models;

namespace CalisthenicsStore.ViewModels.Admin.ProductManagement
{
    public class ProductManagementIndexViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        public bool IsDeleted { get; set; }

        public string? CategoryName { get; set; }
    }
}
