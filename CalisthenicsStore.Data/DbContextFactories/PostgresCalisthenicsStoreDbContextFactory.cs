using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CalisthenicsStore.Data.DbContextFactories
{
    public class PostgresCalisthenicsStoreDbContextFactory : IDesignTimeDbContextFactory<PostgresCalisthenicsStoreDbContext>
    {
        public PostgresCalisthenicsStoreDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PostgresCalisthenicsStoreDbContext>();

            var conn = "Host=dpg-d5qdjt95pdvs7393udp0-a.oregon-postgres.render.com;Database=calisthenics_store_postgres_p2wx;Username=calisthenics_store_postgres_p2wx_user;Password=x6HojWSbzgPChPUxUV4fxXribjBWu13J;SSL Mode=Require;Trust Server Certificate=true";

            optionsBuilder.UseNpgsql(conn);

            return new PostgresCalisthenicsStoreDbContext(optionsBuilder.Options);
        }
    }
}
