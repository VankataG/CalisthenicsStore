using CalisthenicsStore.Data;
using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Data.Repositories.Interfaces;
using CalisthenicsStore.Data.Seeding;
using CalisthenicsStore.Data.Seeding.Interfaces;
using CalisthenicsStore.Data.Utilities;
using CalisthenicsStore.Data.Utilities.Interfaces;
using CalisthenicsStore.Services.Interfaces;
using CalisthenicsStore.Web.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsEnvironment("Render"))
{
    //Add InMemory Database for Render
    builder.Services.AddDbContext<CalisthenicsStoreDbContext>(options =>
        options.UseInMemoryDatabase("CalisthenicsStoreDb"));
}
else
{
    // Add EF Core context
    var connectionString = builder.Configuration.GetConnectionString("CalisthenicsStore") ??
                           throw new InvalidOperationException("Connection string 'CalisthenicsStore' not found.");
    builder.Services.AddDbContext<CalisthenicsStoreDbContext>(options =>
        options.UseSqlServer(
            connectionString,
            sql => sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(3), null)));
}

// Add services to the container.
builder.Services.AddControllersWithViews();
    
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
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
    .AddEntityFrameworkStores<CalisthenicsStoreDbContext>();

builder.Services.AddRazorPages();

builder.Services.AddScoped<IValidator, EntityValidator>();

builder.Services.AddUserDefinedServices(typeof(IProductService).Assembly);
builder.Services.AddRepositories(typeof(IProductRepository).Assembly);
builder.Services.AddTransient<IIdentitySeeder, IdentitySeeder>();


//Adding sessions for the Cart
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var db = services.GetRequiredService<CalisthenicsStoreDbContext>();

    var validator = services.GetRequiredService<IValidator>();
    var dataProcessor = new DataProcessor(validator);

    if (app.Environment.IsEnvironment("Render"))
    {
        db.Database.EnsureCreated();
        await dataProcessor.ImportProductsFromJson(db);
    }
    else
    {
        db.Database.Migrate();
        await dataProcessor.ImportProductsFromJson(db);
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (!app.Environment.IsEnvironment("Render"))
{
    app.UseHttpsRedirection();
}

app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode= {0}"); 
app.UseStaticFiles();

app.UseRouting();

app.SeedDefaultIdentity();

app.UseAuthentication();
app.UseAuthorization();

app.UseAdminRedirection();
app.UseSession(); //Here we use the session needed for the Cart

app.MapControllerRoute(
    name: "areas",
    pattern: "{area}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();


app.Run();
