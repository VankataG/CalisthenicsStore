using CalisthenicsStore.Data.Models;
using CalisthenicsStore.ViewModels.Order;

namespace CalisthenicsStore.Services.Interfaces
{
    public interface IOrderService
    {
        Task<CheckoutViewModel> CheckoutCartItemsAsync();

        Task<int> PlaceOrderAsync(CheckoutViewModel model, string email);
    }
}
