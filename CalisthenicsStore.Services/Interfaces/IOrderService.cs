using CalisthenicsStore.Data.Models;
using CalisthenicsStore.ViewModels.Order;

namespace CalisthenicsStore.Services.Interfaces
{
    public interface IOrderService
    {
        CheckoutViewModel CheckoutCartItems();

        Task<int> PlaceOrderAsync(CheckoutViewModel model);
    }
}
