using Castle.Core.Resource;
using Moq;
using OnlineRetailer.Core;
using OnlineRetailer.Core.Entities;
using OnlineRetailer.Core.Interfaces;
using OnlineRetailer.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRetailer.SpecFlowTests.StepDefinitions
{
    
    [Binding]
    public sealed class ProductStepDefinitions
    {

        private Mock<IRepository<Customer>> mockCustomerRepository;
        private Mock<IRepository<Order>> mockOrderRepository;
        private Mock<IRepository<Product>> mockProductRepository;

        private IOrderManager orderManager;

        List<Product> TestProducts;
        List<OrderLine> TestOrderlines;
        bool AvailabilityResult;

        public ProductStepDefinitions()
        {
            var orderlines = new List<OrderLine>
            {
                new OrderLine{Id = 1, ProductId = 1, Quantity = 1 }
            };
            var orders = new List<Order>
            {
                new Order {Id = 1, CustomerId = 1, OrderDate = DateTime.Now, OrderLines = orderlines}
            };

            var customers = new List<Customer>
            {
                new Customer {Balance = 200, Email = "LarsLarsen@gmail.com",Id = 1, Name = "Lars"}
            };

            var products = new List<Product>
            {
                new Product { Id = 1, ItemsInStock = 5, Name = "Big Hammer",Price = 50}
            };
            mockOrderRepository = new Mock<IRepository<Order>>();
            mockCustomerRepository = new Mock<IRepository<Customer>>();
            mockProductRepository = new Mock<IRepository<Product>>();

            mockOrderRepository.Setup(x => x.GetAll()).Returns(orders);
            mockCustomerRepository.Setup(x => x.GetAll()).Returns(customers);
            mockProductRepository.Setup(x => x.GetAll()).Returns(products);

            orderManager = new OrderManager(mockOrderRepository.Object, mockProductRepository.Object, mockCustomerRepository.Object);
        }
        [Given(@"A product is created")]
        public void GivenAProductIsCreated()
        {
            TestProducts = new List<Product>
            {
                new Product {ItemsInStock = 200, Price = 30, Id = 2, Name = "TestHammers" }
            };


            mockProductRepository.Object.Add(TestProducts[0]);
        }

        [Given(@"There are (.*) items available")]
        public void GivenThereAreItemsAvailable(int p0)
        {
            TestProducts[0].ItemsInStock = p0;
        }

        [Given(@"The customer wants (.*) items")]
        public void GivenTheCustomerWantsItems(int p0)
        {
            TestOrderlines = new List<OrderLine>
            {
                new OrderLine{Id = 1, ProductId = TestProducts[0].Id, Quantity = p0 }
            };
        }


        [When(@"the status is checked")]
        public void WhenTheStatusIsChecked()
        {
            AvailabilityResult = orderManager.CheckAvailability(TestProducts[0], TestOrderlines[0]);
        }

        [Then(@"the availability is (.*)")]
        public void ThenTheAvailabilityIsTrue(bool result)
        {
            Assert.Equal(result, AvailabilityResult);
        }


    }
}
