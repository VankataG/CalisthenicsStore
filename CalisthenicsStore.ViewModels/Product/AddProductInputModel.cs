

using Microsoft.AspNetCore.Mvc.Rendering;

namespace CalisthenicsStore.ViewModels.Product
{
    public class AddProductInputModel
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public string? ImageUrl { get; set; }

        public int CategoryId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
    }
}
