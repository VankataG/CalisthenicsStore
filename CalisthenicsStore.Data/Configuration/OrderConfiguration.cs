﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using CalisthenicsStore.Data.Models;

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
                .HasMaxLength(100);

            entity
                .Property(o => o.Address)
                .IsRequired()
                .HasMaxLength(200);

            entity
                .Property(o => o.City)
                .IsRequired()
                .HasMaxLength(100);

            entity
                .Property(o => o.Email)
                .IsRequired()
                .HasMaxLength(320);

            entity
                .Property(o => o.OrderDate)
                .IsRequired();

            entity
                .Property(o => o.Status)
                .IsRequired()
                .HasMaxLength(30);

        }
    }
}
