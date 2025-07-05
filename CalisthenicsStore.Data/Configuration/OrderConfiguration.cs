using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using CalisthenicsStore.Data.Models;
using static CalisthenicsStore.Common.Constants.Order;

namespace CalisthenicsStore.Data.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> entity)
        {
            entity
                .HasKey(o => o.Id);

            entity
                .Property(o => o.CustomerName)
                .IsRequired()
                .HasMaxLength(CustomerNameMaxLength);

            entity
                .Property(o => o.Address)
                .IsRequired()
                .HasMaxLength(AddressMaxLength);

            entity
                .Property(o => o.City)
                .IsRequired()
                .HasMaxLength(CityMaxLength);

            entity
                .Property(o => o.Email)
                .IsRequired()
                .HasMaxLength(EmailMaxLength);

            entity
                .Property(o => o.OrderDate)
                .IsRequired();

            entity
                .Property(o => o.Status)
                .IsRequired()
                .HasMaxLength(StatusMaxLength);

            entity
                .Property(o => o.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasQueryFilter(o => o.IsDeleted == false);

        }
    }
}
