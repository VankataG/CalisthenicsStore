using CalisthenicsStore.ViewModels.CartItem;

namespace CalisthenicsStore.ViewModels.Order
{
    public class CheckoutViewModel
    {
        public IEnumerable<CartItemViewModel> CartItems { get; set; }

        public decimal TotalPrice { get; set; }
           
    }
}
