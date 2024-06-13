using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using OnlineRetailer.Core.Entities;
using OnlineRetailer.Core.Services;
using OnlineRetailer.Infrastructure;

namespace OnlineRetailer.IntegrationTests
{
    [TestClass]
    public class OrderManagerTests : IDisposable
    {
        SqliteConnection connection;
        OrderManager orderManager;
        OnlineRetailerContext dbContext;

        public OrderManagerTests()
        {
            connection = new SqliteConnection("DataSource=:memory:");

            // In-memory database only exists while the connection is open
            connection.Open();

            // Initialize test database
            var options = new DbContextOptionsBuilder<OnlineRetailerContext>()
                            .UseSqlite(connection).Options;
            dbContext = new OnlineRetailerContext(options);
            IDbInitializer dbInitializer = new DbInitializer();
            dbInitializer.Initialize(dbContext);

            // Create repositories and OrderManager
            var unitOfWork = new UnitOfWork(dbContext);
            orderManager = new OrderManager(unitOfWork);
        }

        public void Dispose()
        {
            // This will delete the in-memory database
            connection.Close();
        }

        /*
         * Test case 1:
         * Tilstrækkelig på lager: Y
         * Tilstrækkelig på saldo: Y
         */
        [TestMethod]
        public void Test_CreateOrder_Success()
        {
            var order = new Order
            {
                CustomerId = 2, //kunde med id 2 har tilstrækkelig saldo
                OrderLines = new List<OrderLine>
                {
                    new OrderLine { ProductId = 1, Quantity = 2 } //produkt id 1 er på lager
                }
            };

            bool result = orderManager.CreateOrder(order);

            Assert.IsTrue(result); //ordren laves
            Assert.AreEqual(3, dbContext.Products.Find(1).ItemsInStock); //produkt med id 1 skal være reduceret med 2 (førhen 5)
            Assert.AreEqual(300, dbContext.Customers.Find(2).Balance); //kundens saldo skal være reduceret til 300
        }
        /*
         * Test case 2:
         * Tilstrækkelig på lager: N
         * Tilstrækkelig på saldo: -
         */
        [TestMethod]
        public void Test_CreateOrder_InsufficientStock()
        {
            var order = new Order
            {
                CustomerId = 2,
                OrderLines = new List<OrderLine>
                {
                    new OrderLine { ProductId = 1, Quantity = 6 } 
                }
            };

            bool result = orderManager.CreateOrder(order);

            Assert.IsFalse(result);
            Assert.AreEqual(5, dbContext.Products.Find(1).ItemsInStock); 
            Assert.AreEqual(500, dbContext.Customers.Find(2).Balance);
        }
        /*
         * Test case 3:
         * Tilstrækkelig på lager: -
         * Tilstrækkelig på saldo: N
         */
        [TestMethod]
        public void Test_CreateOrder_InsufficientBalance()
        {
            var order = new Order
            {
                CustomerId = 1,
                OrderLines = new List<OrderLine>
                {
                    new OrderLine { ProductId = 2, Quantity = 1 }
                }
            };

            bool result = orderManager.CreateOrder(order);

            Assert.IsFalse(result);
            Assert.AreEqual(15, dbContext.Products.Find(2).ItemsInStock);
            Assert.AreEqual(5, dbContext.Customers.Find(1).Balance);
        }
        /*
        * Test case 4:
        * Tilstrækkelig på lager: Min
        * Tilstrækkelig på saldo: Min
        */
        [TestMethod]
        public void Test_CreateOrder_MinBoundaryValues()
        {
            dbContext.Products.Find(1).ItemsInStock = 1;

            var order = new Order
            {
                CustomerId = 2, 
                OrderLines = new List<OrderLine>
                {
                    new OrderLine { ProductId = 1, Quantity = 1 }
                }
            };

            bool result = orderManager.CreateOrder(order);

            Assert.IsTrue(result);
            Assert.AreEqual(0, dbContext.Products.Find(1).ItemsInStock);
            Assert.AreEqual(400, dbContext.Customers.Find(2).Balance); 
        }
        /*
        * Test case 5:
        * Tilstrækkelig på lager: Min - 1
        * Tilstrækkelig på saldo: Min - 1
        */
        [TestMethod]
        public void Test_CreateOrder_MinBoundaryValues_Invalid()
        {
            var order = new Order
            {
                CustomerId = 2,
                OrderLines = new List<OrderLine>
                {
                    new OrderLine { ProductId = 1, Quantity = 0 }
                }
            };

            var exception = Assert.ThrowsException<ArgumentException>(() => orderManager.CreateOrder(order));
            Assert.AreEqual("Order line must have a quantity greater than 0. (Parameter 'OrderLines')", exception.Message);

            Assert.AreEqual(5, dbContext.Products.Find(1).ItemsInStock);
            Assert.AreEqual(500, dbContext.Customers.Find(2).Balance);
        }
        /*
         * Test case 6:
         * Tilstrækkelig på lager: max
         * Tilstrækkelig på saldo: max
         * (Note der er ingen max balance lige nu... Kan eventuelt implementere det alt efter hvad i synes)
         *  Måske burde tilstrækkelig på saldo være en "don't care" => Tilstrækkelig på saldo: - 
         */
        [TestMethod]
        public void Test_CreateOrder_MaxBoundaryValues()
        {
            var order = new Order
            {
                CustomerId = 2, 
                OrderLines = new List<OrderLine>
                {
                    new OrderLine { ProductId = 1, Quantity = 5 } //alle screwdrivers 
                }
            };

            bool result = orderManager.CreateOrder(order);

            Assert.IsTrue(result);
            Assert.AreEqual(0, dbContext.Products.Find(1).ItemsInStock); //0 screwdrivers efter dette
            Assert.AreEqual(0, dbContext.Customers.Find(2).Balance); //0 balance
        }


        /*
         * Test case 7:
         * Tilstrækkelig på lager: max + 1
         * Tilstrækkelig på saldo: max + 1
         * (Note der er ingen max balance lige nu... Kan eventuelt implementere det alt efter hvad i synes)
         * Måske burde tilstrækkelig på saldo være en "don't care" => Tilstrækkelig på saldo: - 
         */
        [TestMethod]
        public void Test_CreateOrder_MaxBoundaryValues_Invalid()
        {
            var order = new Order
            {
                CustomerId = 2,
                OrderLines = new List<OrderLine>
                {
                    new OrderLine { ProductId = 2, Quantity = 16 }
                }
            };

            bool result = orderManager.CreateOrder(order);

            Assert.IsFalse(result);
            Assert.AreEqual(15, dbContext.Products.Find(2).ItemsInStock);
            Assert.AreEqual(500, dbContext.Customers.Find(2).Balance); 
        }
    }
}
