using bi_dashboard_api.Models;
using Microsoft.EntityFrameworkCore;

namespace bi_dashboard_api.Models
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Server> Servers { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the Customer-Order relationship
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId);

            // Your existing server seeding
            modelBuilder.Entity<Server>().HasData(
                new Server { Id = 1, Name = "Production Server", isOnline = true },
                new Server { Id = 2, Name = "Backup Server", isOnline = true },
                new Server { Id = 3, Name = "Development Server", isOnline = false },
                new Server { Id = 4, Name = "Testing Server", isOnline = true },
                new Server { Id = 5, Name = "Staging Server", isOnline = false }
            );
        }
    }
}