using PortraitsByPerminder.Data;
using PortraitsByPerminder.Services;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();

var configuration = builder.Configuration;
var environment = builder.Environment;

string baseUrl = configuration.GetValue<string>("BaseUrl") ?? "https://localhost:5001/";

builder.Services.AddHttpClient("DefaultClient", client =>
{
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("DefaultClient"));

builder.Services.AddDbContext<PizzaStoreContext>(options =>
    options.UseSqlite("Data Source=pizza.db"));
builder.Services.AddScoped<OrderState>();
builder.Services.AddDbContext<PhotoContext>(options => 
    options.UseSqlite("Data Source=portraitsbyperminder.db"));
builder.Services.AddScoped<PhotoService>();

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

// Initialize the database with detailed logging
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    var pizzaDb = scope.ServiceProvider.GetRequiredService<PizzaStoreContext>();
    try
    {
        if (pizzaDb.Database.EnsureCreated())
        {
            Console.WriteLine("Pizza database created successfully.");
            SeedData.Initialize(pizzaDb);
            Console.WriteLine("Pizza database seeded successfully.");
        }
        else
        {
            Console.WriteLine("Pizza database already exists.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while initializing the photo database: {ex.Message}");
        throw; // Re-throw the exception to ensure it's not swallowed
    }

    var photoDb = scope.ServiceProvider.GetRequiredService<PhotoContext>();
    try
    {
        if (photoDb.Database.EnsureCreated())
        {
            Console.WriteLine("Photo database created successfully.");
            SeedData.InitializePhotoDb(photoDb);
            Console.WriteLine("Photo database seeded successfully.");
        }
        else
        {
            Console.WriteLine("Photo database already exists.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while initializing the photo database: {ex.Message}");
        throw; // Re-throw the exception to ensure it's not swallowed
    }
}

app.Run();
