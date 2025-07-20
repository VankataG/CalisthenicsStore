namespace CalisthenicsStore.Data.Models
{
    public class OrderProduct
    {
        public Guid ProductId { get; set; }

        public Product Product { get; set; } = null!;

        public int Quantity { get; set; } 

        public decimal UnitPrice { get; set; }

        public bool IsDeleted { get; set; }

        public Guid OrderId { get; set; }

        public Order Order { get; set; } = null!;
    }
}
