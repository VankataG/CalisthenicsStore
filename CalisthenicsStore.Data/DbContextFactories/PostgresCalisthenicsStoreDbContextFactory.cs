using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CalisthenicsStore.Data.DbContextFactories
{
    public class PostgresCalisthenicsStoreDbContextFactory : IDesignTimeDbContextFactory<PostgresCalisthenicsStoreDbContext>
    {
        public PostgresCalisthenicsStoreDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<PostgresCalisthenicsStoreDbContextFactory>(optional: true)
                .AddEnvironmentVariables()
                .Build();

            var conn = configuration.GetConnectionString("CalisthenicsStorePostgres");

            if (string.IsNullOrWhiteSpace(conn))
            {
                throw new InvalidOperationException("Connection string 'ConnectionStrings:CalisthenicsStorePostgres' was not found. " +
                    "Set it in User Secrets (local) or as an environment variable (ConnectionStrings__CalisthenicsStorePostgres).");
            }


            var optionsBuilder = new DbContextOptionsBuilder<PostgresCalisthenicsStoreDbContext>();
            optionsBuilder.UseNpgsql(conn);

            return new PostgresCalisthenicsStoreDbContext(optionsBuilder.Options);
        }
    }
}
