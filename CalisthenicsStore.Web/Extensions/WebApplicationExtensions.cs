using CalisthenicsStore.Data;
using CalisthenicsStore.Data.Seeding.Interfaces;
using CalisthenicsStore.Data.Utilities;
using CalisthenicsStore.Data.Utilities.Interfaces;
using CalisthenicsStore.Web.Middlewares;
using Microsoft.EntityFrameworkCore;

namespace CalisthenicsStore.Web.Extensions
{
    public static class WebApplicationExtensions
    {
        public static IApplicationBuilder SeedDefaultIdentity(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            IServiceProvider serviceProvider = scope.ServiceProvider;

            IIdentitySeeder identitySeeder = serviceProvider.GetRequiredService<IIdentitySeeder>();

            identitySeeder
                .SeedIdentityAsync()
                .GetAwaiter()
                .GetResult();

            return app;
        }

        public static IApplicationBuilder UseAdminRedirection(this IApplicationBuilder app)
        {
            app.UseMiddleware<AdminRedirectionMiddleware>();

            return app;
        }

        public static async Task ApplyMigrationsAndSeedDataAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var validator = services.GetRequiredService<IValidator>();
                var dataProcessor = new DataProcessor(validator);

                CalisthenicsStoreDbContext db = services.GetRequiredService<CalisthenicsStoreDbContext>();
               
                await db.Database.MigrateAsync();
                await dataProcessor.ImportProductsFromJson(db);
            }
        }
    }
}
