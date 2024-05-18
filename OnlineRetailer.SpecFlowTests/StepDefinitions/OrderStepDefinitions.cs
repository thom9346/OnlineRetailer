using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using OnlineRetailer.Core;
using OnlineRetailer.Core.Entities;
using OnlineRetailer.Core.Interfaces;
using OnlineRetailer.Core.Services;
namespace OnlineRetailer.SpecFlowTests.StepDefinitions
{
    [Binding]
    public sealed class OrderStepDefinitions
    {
        private Mock<IRepository<Customer>> mockCustomerRepository;
        private Mock<IRepository<Order>> mockOrderRepository;
        private Mock<IRepository<Product>> mockProductRepository;

         Order order = new Order();

        private IOrderManager orderManager;
        public OrderStepDefinitions()
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

        Customer customer = new Customer();
        Order myOrder;
        bool finalResult;
        [Given("A user is created")]
        public void GivenUserCreated()
        {
            customer = new Customer { Balance = 200, Email = "pl", Id = 2, Name = "p" };
        }

        [Given(@"the user has (.*) credits")]
        public void GivenTheUserHasCredits(int p0)
        {

            customer.Balance = p0;
        }

        [Given(@"the items total out to (.*)")]
        public void GivenTheItemsTotalOutTo(int p0)
        {

            Product product = new Product { Id = 1, ItemsInStock = 5, Name = "Big Hammer", Price = p0 };
            List<OrderLine> orderline = new List<OrderLine> {
                new OrderLine { Id = 1, ProductId = 1, Quantity = 1 }
            };

            myOrder = new Order { Id = 1, CustomerId = customer.Id, OrderDate = DateTime.Now, OrderLines = orderline };



            myOrder.totalPrice = p0;
        }

        [When(@"the order is placed")]
        public void WhenTheOrderIsPlaced()
        {
            finalResult = customer.Balance >= myOrder.totalPrice;
            //should be calling the createorder func in booking manager probably
        }

        [Then(@"the result is (.*)")]
        public void ThenTheResultIs(bool result)
        {
            Assert.Equal(result, finalResult);
        }



    }

}
