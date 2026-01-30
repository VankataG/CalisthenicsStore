using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

using static CalisthenicsStore.Common.Constants.Product;
using static CalisthenicsStore.Common.ErrorMessages.Product;

namespace CalisthenicsStore.ViewModels.Admin.ProductManagement
{
    public class ProductInputModel
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = NameRequiredError)]
        [MaxLength(NameMaxLength, ErrorMessage = NameMaxLengthError)]
        public string Name { get; set; } = null!;

        [MaxLength(DescriptionMaxLength, ErrorMessage = DescriptionMaxLengthError)]
        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }


        [Url(ErrorMessage = ImageUrlInvalidError)]
        [MaxLength(ImageUrlMaxLength, ErrorMessage = ImageUrlMaxLengthError)]
        public IFormFile? ImageFile { get; set; }

        public Guid CategoryId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
    }
}
