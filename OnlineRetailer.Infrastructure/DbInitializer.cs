using OnlineRetailer.Core;
using OnlineRetailer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineRetailer.Infrastructure
{
    public class DbInitializer : IDbInitializer
    {
        // This method will create and seed the database.
        public void Initialize(OnlineRetailerContext context)
        {
            // Delete the database, if it already exists. I do this because an
            // existing database may not be compatible with the entity model,
            // if the entity model was changed since the database was created.
            context.Database.EnsureDeleted();

            // Create the database, if it does not already exists. This operation
            // is necessary, if you don't use the in-memory database.
            context.Database.EnsureCreated();

            // Look for any bookings.
            if (context.Product.Any())
            {
                return;   // DB has been seeded
            }

            List<Customer> customers = new List<Customer>
            {
                new Customer { Name="John Smith", Email="js@gmail.com" },
                new Customer { Name="Jane Doe", Email="jd@gmail.com" }
            };

            List<Product> rooms = new List<Product>
            {
                new Product { Name = "hammer", Price = 100, ItemsInStock = 5 },
                new Product { Name = "screwdriver", Price = 200, ItemsInStock = 15 },
                new Product { Name = "søm", Price = 500, ItemsInStock = 0 }
            };

            DateTime date = DateTime.Today;
            List<Order> orders = new List<Order>
            {
                new Order { CustomerId = 1, ProductId = 1, Quantity = 2, OrderDate = date },
                new Order { CustomerId = 1, ProductId = 2, Quantity = 5, OrderDate = date },
                new Order { CustomerId = 2, ProductId = 1, Quantity = 4, OrderDate = date }
            };

            context.Customer.AddRange(customers);
            context.Product.AddRange(rooms);
            context.SaveChanges();
            context.Order.AddRange(orders);
            context.SaveChanges();
        }
    }
}
