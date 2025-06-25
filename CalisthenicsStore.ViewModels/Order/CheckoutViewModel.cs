using CalisthenicsStore.ViewModels.CartItem;

namespace CalisthenicsStore.ViewModels.Order
{
    public class CheckoutViewModel
    {
        public IEnumerable<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();

        public string CustomerName { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Address { get; set; } = null!;

        public decimal TotalPrice { get; set; }
           
    }
}
