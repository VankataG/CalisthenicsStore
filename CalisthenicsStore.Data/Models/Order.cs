namespace CalisthenicsStore.Data.Models
{
    public class Order
    {
        public Guid Id;

        public Guid ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Address { get; set; } = null!;

        public DateTime OrderDate { get; set; } 

        public string Status { get; set; } = null!;

        public bool IsDeleted { get; set; }

        public virtual ICollection<OrderProduct> Products { get; set; } 
            = new HashSet<OrderProduct>();
    }
}
