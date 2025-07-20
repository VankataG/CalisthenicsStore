using System.ComponentModel.DataAnnotations;

namespace CalisthenicsStore.ViewModels.Product
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        [Display(Name = "Product Name")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Field {0} must be between {2} and {1} symbols")]
        public string Name { get; set; } = "No description";

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        //public int StockQuantity { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;


    }
}
