using CalisthenicsStore.Data;
using CalisthenicsStore.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace CalisthenicsStore.Web.Extensions
{
    public static class DbAndIdentityExtensions
    {
        public static IServiceCollection AddDatabaseAndIdentity(this IServiceCollection services, IConfiguration config, IHostEnvironment env)
        {
            if (env.IsEnvironment("Render"))
            {
                services.AddRenderPostgres(config);
            }
            else
            {
                services.AddLocalSqlServer(config);
            }

            return services;
        }

        private static IServiceCollection AddRenderPostgres(this IServiceCollection services, IConfiguration config)
        {
            //Add InMemory Database for Render
            //services.AddDbContext<CalisthenicsStoreDbContext>(options =>
            //    options.UseInMemoryDatabase("CalisthenicsStoreDb"));


            var connectionString = config.GetConnectionString("CalisthenicsStorePostgres")
                            ?? throw new InvalidOperationException("PostgreSQL connection string not found.");

            services.AddDbContext<PostgresCalisthenicsStoreDbContext>(options =>
                options.UseNpgsql(connectionString, npgsql =>
                    npgsql.MigrationsAssembly(typeof(PostgresCalisthenicsStoreDbContext).Assembly.FullName)
                        .MigrationsHistoryTable("__EFMigrationsHistory", "public")));

            services.AddScoped<CalisthenicsStoreDbContext>(sp =>
                sp.GetRequiredService<PostgresCalisthenicsStoreDbContext>());

            services.AddAppIdentity<PostgresCalisthenicsStoreDbContext>();

            return services;
        }

        private static IServiceCollection AddLocalSqlServer(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("CalisthenicsStore") ??
                           throw new InvalidOperationException("SQL Server connection string not found.");

            services.AddDbContext<SqlServerCalisthenicsStoreDbContext>(options =>
                options.UseSqlServer(connectionString, sql =>
                    sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(3), null)
                        .MigrationsAssembly(typeof(SqlServerCalisthenicsStoreDbContext).Assembly.FullName)));

            services.AddScoped<CalisthenicsStoreDbContext>(sp =>
                sp.GetRequiredService<SqlServerCalisthenicsStoreDbContext>());

            services.AddAppIdentity<SqlServerCalisthenicsStoreDbContext>();

            return services;
        }

        private static IServiceCollection AddAppIdentity<TContext>(this IServiceCollection services) where TContext : DbContext
        {
            services
                .AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.SignIn.RequireConfirmedEmail = false;
                    options.SignIn.RequireConfirmedPhoneNumber = false;

                    //Add requirements for the password
                    options.Password.RequireDigit = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 5;

                })
                .AddEntityFrameworkStores<TContext>();

            return services;
        }
    }
}
