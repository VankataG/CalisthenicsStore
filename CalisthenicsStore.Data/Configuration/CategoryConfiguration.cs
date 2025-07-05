
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using CalisthenicsStore.Data.Models;
using static CalisthenicsStore.Common.Constants.Category;

namespace CalisthenicsStore.Data.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> entity)
        {
            entity
                .HasKey(c => c.Id);

            entity
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(NameMaxLength);

            entity
                .Property(c => c.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasQueryFilter(c => c.IsDeleted == false);

            entity
                .HasData(this.SeedCategories());
        }

        public List<Category> SeedCategories()
        {
            List<Category> categories = new List<Category>()
            {
                new Category { Id = 1, Name = "Bars" },
                new Category { Id = 2, Name = "Rings" },
                new Category { Id = 3, Name = "Parallettes" },
                new Category { Id = 4, Name = "Resistance Bands" },
                new Category { Id = 5, Name = "Weighted Vests" },
                new Category { Id = 6, Name = "Grip Trainers" },
                new Category { Id = 7, Name = "Floor Mats" },
                new Category { Id = 8, Name = "Wrist & Lifting Wraps" },
                new Category { Id = 9, Name = "Training Apparel" },
                new Category { Id = 10, Name = "Books & Training Guides" }

            };
            return categories;
        }
    }
}
