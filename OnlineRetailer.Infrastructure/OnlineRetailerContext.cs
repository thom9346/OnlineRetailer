
using OnlineRetailer.Core;
using Microsoft.EntityFrameworkCore;
using OnlineRetailer.Core.Entities;

namespace OnlineRetailer.Infrastructure
{
    public class OnlineRetailerContext : DbContext
    {
        public OnlineRetailerContext(DbContextOptions<OnlineRetailerContext> options)
            : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

    }
}
