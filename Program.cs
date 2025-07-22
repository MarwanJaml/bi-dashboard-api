using bi_dashboard_api.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Diagnostics;
using System.Text.Json.Serialization;






var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Configure DbContext
builder.Services.AddDbContext<ApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BI Dashboard API", Version = "v1" });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowAllOrigins");
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BI Dashboard API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// ============================================
// APPLICATION STARTUP SEEDING SECTION
// ============================================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApiContext>();
    try
    {
        // Apply migrations first
        db.Database.Migrate();
        Console.WriteLine("Database migrations applied successfully!");

        // SEED DATA HERE - This is the Application Startup Seeding
        await SeedDatabase(db);

        // Log final counts
        var customersCount = db.Customers.Count();
        var serversCount = db.Servers.Count();
        var ordersCount = db.Orders.Count();
        Console.WriteLine($"Database ready - Customers: {customersCount}, Servers: {serversCount}, Orders: {ordersCount}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database setup failed: {ex.Message}");
    }
}

// Auto-launch browser in development mode
if (app.Environment.IsDevelopment())
{
    var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
    lifetime.ApplicationStarted.Register(() =>
    {
        var urls = app.Urls;
        if (urls.Any())
        {
            var url = urls.First().Replace("*", "localhost");
            // Launch Swagger UI
            var swaggerUrl = $"{url}/swagger";

            Console.WriteLine($"🚀 Opening browser at: {swaggerUrl}");

            try
            {
                // Cross-platform way to open browser
                var psi = new ProcessStartInfo
                {
                    FileName = swaggerUrl,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not open browser automatically: {ex.Message}");
                Console.WriteLine($"Please manually open: {swaggerUrl}");
            }
        }
    });
}

app.Run();

// ============================================
// SEEDING METHOD - This is where the magic happens
// ============================================
async Task SeedDatabase(ApiContext context)
{
    Console.WriteLine("Starting database seeding...");

    // Seed customers if we don't have any
    if (!context.Customers.Any())
    {
        var customers = new List<Customer>
        {
            new Customer { Name = "John Doe", Email = "john.doe@example.com", Status = "Active" },
            new Customer { Name = "Jane Smith", Email = "jane.smith@example.com", Status = "Active" },
            new Customer { Name = "Bob Johnson", Email = "bob.johnson@example.com", Status = "Active" },
            new Customer { Name = "Alice Brown", Email = "alice.brown@example.com", Status = "Inactive" },
            new Customer { Name = "David Wilson", Email = "david.wilson@example.com", Status = "Active" },
            new Customer { Name = "Sarah Miller", Email = "sarah.miller@example.com", Status = "Active" },
            new Customer { Name = "Michael Davis", Email = "michael.davis@example.com", Status = "Inactive" },
            new Customer { Name = "Emily Johnson", Email = "emily.johnson@example.com", Status = "Active" },
            new Customer { Name = "Chris Lee", Email = "chris.lee@example.com", Status = "Active" },
            new Customer { Name = "Lisa Wang", Email = "lisa.wang@example.com", Status = "Active" }
        };

        context.Customers.AddRange(customers);
        await context.SaveChangesAsync();
        Console.WriteLine($"✅ Seeded {customers.Count} customers");
    }
    else
    {
        Console.WriteLine("ℹ️  Customers already exist, skipping customer seeding");
    }

    // Seed orders if we don't have any
    if (!context.Orders.Any())
    {
        var customers = context.Customers.ToList();
        var random = new Random();
        var orders = new List<Order>();

        // Generate 50 random orders with realistic data
        for (int i = 0; i < 50; i++)
        {
            var customer = customers[random.Next(customers.Count)];
            var placedDate = DateTime.Now.AddDays(-random.Next(1, 180)); // Orders from last 6 months

            // 80% chance the order is completed
            var completedDate = random.Next(1, 10) > 2
                ? placedDate.AddDays(random.Next(1, 14)) // Completed 1-14 days after placed
                : (DateTime?)null; // 20% not completed

            orders.Add(new Order
            {
                CustomerId = customer.Id,
                Total = Math.Round((decimal)(random.NextDouble() * 800 + 20), 2), // $20-$820
                Placed = placedDate,
                Completed = completedDate
            });
        }

        context.Orders.AddRange(orders);
        await context.SaveChangesAsync();
        Console.WriteLine($"✅ Seeded {orders.Count} orders");
    }
    else
    {
        Console.WriteLine("ℹ️  Orders already exist, skipping order seeding");
    }

    Console.WriteLine("Database seeding completed!");
}