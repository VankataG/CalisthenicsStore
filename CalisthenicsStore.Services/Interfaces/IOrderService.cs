using CalisthenicsStore.Data.Models;
using CalisthenicsStore.ViewModels.Order;
using CalisthenicsStore.ViewModels.Payment;

namespace CalisthenicsStore.Services.Interfaces
{
    public interface IOrderService
    {
        Task<CheckoutViewModel> CheckoutCartItemsAsync();

        Task<Guid> PlaceOrderAsync(CheckoutViewModel model, string userId);

        Task<PaymentViewModel?> GetPaymentViewModelAsync(Guid orderId);

        Task MarkOrderAsPaidAsync(Guid orderId);

        Task<string?> GetOrderStatusAsync(Guid orderId);
    }
}
