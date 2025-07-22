
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using CalisthenicsStore.Data.Models;
using static CalisthenicsStore.Common.Constants.ApplicationUser;

namespace CalisthenicsStore.Data.Configuration
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> entity)
        {
            entity
                .Property(au => au.FirstName)
                .IsRequired()
                .HasMaxLength(FirstNameMaxLength);

            entity
                .Property(au => au.LastName)
                .IsRequired()
                .HasMaxLength(LastNameMaxLength);

            
        }
    }
}
