using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using CalisthenicsStore.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace CalisthenicsStore.Data
{
    public class CalisthenicsStoreDbContext : IdentityDbContext<IdentityUser>
    {
        public CalisthenicsStoreDbContext(DbContextOptions<CalisthenicsStoreDbContext> options)
                : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<OrderProduct> OrderProducts { get; set; }

        public virtual DbSet<Exercise> Exercises { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}
