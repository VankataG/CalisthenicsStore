
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
                new Category { Id = Guid.Parse("5a5e8602-4521-4a8a-b0c6-9b1180a6bafc"), Name = "Bars" },
                new Category { Id = Guid.Parse("26a82284-2ce9-40b1-a92a-1ed2f744f2b5"), Name = "Rings" },
                new Category { Id = Guid.Parse("e8e47bf6-78e7-4fee-b9d5-d21cb551e0c8"), Name = "Parallettes" },
                new Category { Id = Guid.Parse("7722f1a4-07c5-4385-a868-4592c2235928"), Name = "Resistance Bands" },
                new Category { Id = Guid.Parse("fe94fada-1ab8-4892-83a3-216639c03579"), Name = "Weighted Vests" },
                new Category { Id = Guid.Parse("c82d3fda-b115-4c76-91da-7f7bc722eb1c"), Name = "Grip Trainers" },
                new Category { Id = Guid.Parse("a74a64ea-32ab-48b0-a915-a363ca3c6dbd"), Name = "Floor Mats" },
                new Category { Id = Guid.Parse("529d56fc-1398-4cb3-8890-1e56209f951f"), Name = "Wrist & Lifting Wraps" },
                new Category { Id = Guid.Parse("95fa052b-0876-4e21-b1c6-669de1fddcea"), Name = "Training Apparel" },
                new Category { Id = Guid.Parse("435dd8b6-28b5-48e9-b3b5-5bea8c19a4e4"), Name = "Books & Training Guides" }

            };
            return categories;
        }
    }
}
