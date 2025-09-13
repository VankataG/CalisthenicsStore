using CalisthenicsStore.Data.Models;
using CalisthenicsStore.ViewModels.Product;

namespace CalisthenicsStore.ViewModels.Order
{
    public class ProfileOrderViewModel
    {
        public Guid Id { get; set; }

        public Guid ApplicationUserId { get; set; }

        public string City { get; set; } = null!;

        public string Address { get; set; } = null!;

        public DateTime OrderDate { get; set; }

        public string Status { get; set; } = null!;

        public IEnumerable<ProfileProductViewModel> Products { get; set; }
            = Enumerable.Empty<ProfileProductViewModel>();

    }
}
