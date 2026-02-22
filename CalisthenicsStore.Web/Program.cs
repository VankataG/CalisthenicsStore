using CalisthenicsStore.Data.Repositories.Interfaces;
using CalisthenicsStore.Data.Seeding;
using CalisthenicsStore.Data.Seeding.Interfaces;
using CalisthenicsStore.Data.Utilities;
using CalisthenicsStore.Data.Utilities.Interfaces;
using CalisthenicsStore.Services.Interfaces;
using CalisthenicsStore.Web.Extensions;
using Stripe;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabaseAndIdentity(builder.Configuration, builder.Environment);

builder.Services.AddControllersWithViews();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddRazorPages();

builder.Services.AddScoped<IValidator, EntityValidator>();
builder.Services.AddScoped<DataProcessor>();
builder.Services.AddUserDefinedServices(typeof(IProductService).Assembly);
builder.Services.AddRepositories(typeof(IProductRepository).Assembly);
builder.Services.AddTransient<IIdentitySeeder, IdentitySeeder>();

builder.Services.AddSupabase(builder.Configuration);
builder.Services.AddReCaptcha(builder.Configuration);
builder.Services.AddStripe(builder.Configuration);
builder.Services.AddCartSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

if (!app.Environment.IsEnvironment("Render"))
{
    app.UseHttpsRedirection();
}

app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode={0}"); 
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.SeedDefaultIdentity();

app.UseAuthentication();
app.UseAuthorization();

app.UseAdminRedirection();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

//Configure Stripe secret key from configuration
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe")["SecretKey"];

//Startup work
await app.ApplyMigrationsAndSeedDataAsync();

app.Run();
