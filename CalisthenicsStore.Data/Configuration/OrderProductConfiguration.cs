using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using CalisthenicsStore.Data.Models;

namespace CalisthenicsStore.Data.Configuration
{
    public class OrderProductConfiguration : IEntityTypeConfiguration<OrderProduct>
    {
        public void Configure(EntityTypeBuilder<OrderProduct> entity)
        {
            entity
                .HasKey(oi => oi.Id);

            entity
                .Property(oi => oi.Quantity)
                .IsRequired();

            entity
                .Property(oi => oi.UnitPrice)
                .IsRequired();

            entity
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(oi => oi.Order)
                .WithMany(o => o.Products)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
