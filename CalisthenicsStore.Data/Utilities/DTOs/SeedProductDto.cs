namespace CalisthenicsStore.Data.Utilities.DTOs
{
    public sealed class SeedProductDto
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string? ImagePath { get; set; }

        public Guid CategoryId { get; set; }

        public int StockQuantity { get; set; }
    }
}
