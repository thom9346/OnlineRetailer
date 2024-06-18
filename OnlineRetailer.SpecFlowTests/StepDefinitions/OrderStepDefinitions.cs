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

        private readonly Mock<IUnitOfWork> mockUnitOfWork;

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
            mockUnitOfWork = new Mock<IUnitOfWork>();

            mockOrderRepository = new Mock<IRepository<Order>>();
            mockCustomerRepository = new Mock<IRepository<Customer>>();
            mockProductRepository = new Mock<IRepository<Product>>();

            mockOrderRepository.Setup(x => x.GetAll()).Returns(orders);
            mockCustomerRepository.Setup(x => x.GetAll()).Returns(customers);
            mockProductRepository.Setup(x => x.GetAll()).Returns(products);

            mockCustomerRepository.Setup(repo => repo.Get(It.IsAny<int>()))
    .Returns((int id) => customers.Find(c => c.Id == id));

            mockCustomerRepository.Setup(repo => repo.Add(It.IsAny<Customer>()))
    .Callback((Customer customer) => customers.Add(customer));

            mockProductRepository.Setup(repo => repo.Get(It.IsAny<int>()))
    .Returns((int id) => products.Find(c => c.Id == id));

            mockProductRepository.Setup(repo => repo.Add(It.IsAny<Product>()))
.Callback((Product product) => products.Add(product));

            mockUnitOfWork.Setup(uow => uow.Orders).Returns(mockOrderRepository.Object);
            mockUnitOfWork.Setup(uow => uow.Products).Returns(mockProductRepository.Object);
            mockUnitOfWork.Setup(uow => uow.Customers).Returns(mockCustomerRepository.Object);

            orderManager = new OrderManager(mockUnitOfWork.Object);
        }

        Customer customer = new Customer();
        Order myOrder;
        bool finalResult;

        [Given("A user is created")]
        public void GivenUserCreated()
        {
            customer = new Customer { Balance = 200, Email = "pl", Id = 2, Name = "p" };

            mockCustomerRepository.Object.Add(customer);
        }

        [Given(@"the user has (.*) credits")]
        public void GivenTheUserHasCredits(int p0)
        {

            customer.Balance = p0;
        }

        [Given(@"the items total out to (.*)")]
        public void GivenTheItemsTotalOutTo(int p0)
        {

            Product product = new Product { Id = 2, ItemsInStock = 5, Name = "Big Hammer", Price = p0 };
            List<OrderLine> orderline = new List<OrderLine> {
                new OrderLine { Id = 1, ProductId = product.Id, Quantity = 1 }
            };

            myOrder = new Order { Id = 1, CustomerId = customer.Id, OrderDate = DateTime.Now, OrderLines = orderline };

            mockProductRepository.Object.Add(product);

            myOrder.TotalPrice = p0;
        }

        [When(@"the order is placed")]
        public void WhenTheOrderIsPlaced()
        {
            //finalResult = customer.Balance >= myOrder.TotalPrice;
            //should be calling the createorder func in order manager probably
            finalResult = orderManager.CreateOrder(myOrder);
        }

        [Then(@"the result is (.*)")]
        public void ThenTheResultIs(bool result)
        {
            Assert.Equal(result, finalResult);
        }



    }

}
