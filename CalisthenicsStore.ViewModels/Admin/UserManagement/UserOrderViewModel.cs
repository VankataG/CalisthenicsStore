namespace CalisthenicsStore.ViewModels.Admin.UserManagement
{
    public class UserOrderViewModel
    {
        public Guid Id { get; set; }

        public string City { get; set; } = null!;

        public string Address { get; set; } = null!;

        public DateTime OrderDate { get; set; }

        public string Status { get; set; } = null!;

        public IEnumerable<UserOrderProductViewModel> Products { get; set; } = new HashSet<UserOrderProductViewModel>();
        
    }
}
