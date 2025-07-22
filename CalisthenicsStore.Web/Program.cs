using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using CalisthenicsStore.Data;
using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Data.Repositories.Interfaces;
using CalisthenicsStore.Data.Utilities;
using CalisthenicsStore.Data.Utilities.Interfaces;
using CalisthenicsStore.Services.Interfaces;
using CalisthenicsStore.Web.Extensions;


var builder = WebApplication.CreateBuilder(args);

// Add EF Core context
var connectionString = builder.Configuration.GetConnectionString("CalisthenicsStore") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<CalisthenicsStoreDbContext>(options =>
    options.UseSqlServer(connectionString));

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


//Adding sessions for the Cart
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var app = builder.Build();

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

app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode= {0}"); 
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession(); //Here we use the session needed for the Cart

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    IServiceProvider services = scope.ServiceProvider;

    CalisthenicsStoreDbContext dbContext = services.GetRequiredService<CalisthenicsStoreDbContext>();

    //Here we add the EntityValidator which we get from the services collection
    IValidator entityValidator = services.GetRequiredService<IValidator>();

    DataProcessor dataProcessor = new DataProcessor(entityValidator);

    //TODO: Fix the Import Method
    //await dataProcessor.ImportProductsFromJson(dbContext);
}


app.Run();
