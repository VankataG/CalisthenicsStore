using System.ComponentModel.DataAnnotations;
using CalisthenicsStore.ViewModels.CartItem;

namespace CalisthenicsStore.ViewModels.Payment
{
    public class PaymentViewModel
    {
        public IEnumerable<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();

        public decimal TotalPrice { get; set; }
    }
}
