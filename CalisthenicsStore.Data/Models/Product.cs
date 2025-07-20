﻿namespace CalisthenicsStore.Data.Models
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsDeleted { get; set; }

        public Guid CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}
