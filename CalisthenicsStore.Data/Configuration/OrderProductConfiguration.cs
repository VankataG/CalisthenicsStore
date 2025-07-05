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
                .HasKey(op => new{ op.OrderId, op.ProductId});

            entity
                .Property(op => op.Quantity)
                .IsRequired();

            entity
                .Property(op => op.UnitPrice)
                .HasPrecision(18,2)
                .IsRequired();

            entity
                .Property(op => op.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasQueryFilter(op => op.IsDeleted == false);

            entity
                .HasOne(op => op.Product)
                .WithMany()
                .HasForeignKey(op => op.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(op => op.Order)
                .WithMany(o => o.Products)
                .HasForeignKey(op => op.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
