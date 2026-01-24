using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CalisthenicsStore.Data
{
    public class SqlServerCalisthenicsStoreDbContext : CalisthenicsStoreDbContext
    {
        public SqlServerCalisthenicsStoreDbContext(DbContextOptions<SqlServerCalisthenicsStoreDbContext> options)
            : base(options)
        {
        }
    }

    public class PostgresCalisthenicsStoreDbContext : CalisthenicsStoreDbContext
    {
        public PostgresCalisthenicsStoreDbContext(DbContextOptions<PostgresCalisthenicsStoreDbContext> options)
            : base(options)
        {
        }
    }
}
