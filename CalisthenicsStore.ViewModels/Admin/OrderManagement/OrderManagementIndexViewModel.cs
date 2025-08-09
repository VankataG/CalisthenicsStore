namespace CalisthenicsStore.ViewModels.Admin.OrderManagement
{
    public class OrderManagementIndexViewModel
    {
        public Guid Id { get; set; }

        public string CustomerName { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Address { get; set; } = null!;

        public DateTime OrderDate { get; set; }

        public string Status { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}
