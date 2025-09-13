namespace CalisthenicsStore.ViewModels.Product
{
    public class ProfileProductViewModel
    {
        public Guid ProductId { get; set; }

        public string Name { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal Total { get; set; }
    }
}
