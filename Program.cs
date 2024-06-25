using PortraitsByPerminder.Data;
using PortraitsByPerminder.Services;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();
builder.Services.AddSqlite<PizzaStoreContext>("Data Source=pizza.db");
builder.Services.AddScoped<OrderState>();

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
    var db = scope.ServiceProvider.GetRequiredService<PizzaStoreContext>();
    try
    {
        if (db.Database.EnsureCreated())
        {
            Console.WriteLine("Database created successfully.");
            SeedData.Initialize(db);
            Console.WriteLine("Database seeded successfully.");
        }
        else
        {
            Console.WriteLine("Database already exists.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while initializing the database: {ex.Message}");
        throw; // Re-throw the exception to ensure it's not swallowed
    }
}

app.Run();
