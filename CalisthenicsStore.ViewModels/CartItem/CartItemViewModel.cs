﻿namespace CalisthenicsStore.ViewModels.CartItem
{
    public class CartItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;

        public string? ImageUrl { get; set; }   

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
