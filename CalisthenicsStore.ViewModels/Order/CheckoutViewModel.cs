using System.ComponentModel.DataAnnotations;
using CalisthenicsStore.ViewModels.CartItem;

namespace CalisthenicsStore.ViewModels.Order
{
    public class CheckoutViewModel
    {
        public IEnumerable<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();


        [Required(ErrorMessage = "Please enter your city.")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "Please enter your address.")]
        public string Address { get; set; } = null!;

        public decimal TotalPrice { get; set; }
           
    }
}
