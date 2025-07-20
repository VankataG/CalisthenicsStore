

using Microsoft.AspNetCore.Mvc.Rendering;

namespace CalisthenicsStore.ViewModels.Product
{
    public class ProductInputModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public string? ImageUrl { get; set; }

        public Guid CategoryId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
    }
}
