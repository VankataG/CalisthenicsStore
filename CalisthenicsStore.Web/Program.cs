using CalisthenicsStore.Data;
using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Data.Models.ReCaptcha;
using CalisthenicsStore.Data.Repositories.Interfaces;
using CalisthenicsStore.Data.Seeding;
using CalisthenicsStore.Data.Seeding.Interfaces;
using CalisthenicsStore.Data.Utilities;
using CalisthenicsStore.Data.Utilities.Interfaces;
using CalisthenicsStore.Services;
using CalisthenicsStore.Services.Interfaces;
using CalisthenicsStore.Web.Extensions;
using CalisthenicsStore.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabaseAndIdentity(builder.Configuration, builder.Environment);

//Add Supabase storage
builder.Services.AddSingleton(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();

    var url = config["Supabase:Url"];
    var key = config["Supabase:ServiceRoleKey"];
    var options = new Supabase.SupabaseOptions
    {
        AutoConnectRealtime = false,
    };

    return new Supabase.Client(url!, key, options);
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
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

//ReCaptcha
builder.Services.Configure<GoogleReCaptchaSettings>(builder.Configuration.GetSection("GoogleReCaptcha"));
builder.Services.AddHttpClient<IReCaptchaServ, ReCaptchaServ>();

//Configure StripeSettings using values from appsettings.json
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var validator = services.GetRequiredService<IValidator>();
    var dataProcessor = new DataProcessor(validator);

    CalisthenicsStoreDbContext db;
    if (app.Environment.IsEnvironment("Render"))
    {
        db = services.GetRequiredService<PostgresCalisthenicsStoreDbContext>();
    }
    else
    {
        db = services.GetRequiredService<SqlServerCalisthenicsStoreDbContext>();
    }

    db.Database.Migrate();
    await dataProcessor.ImportProductsFromJson(db);
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

app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode={0}"); 
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

//Configure Stripe secret key from configuration
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe")["SecretKey"];

app.Run();
