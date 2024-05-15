using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using OnlineRetailer.Core;
using OnlineRetailer.Core.Entities;
namespace OnlineRetailer.SpecFlowTests.StepDefinitions
{
    [Binding]
    public sealed class OrderStepDefinitions
    {

        private Mock<IRepository<Order>> mockOrderRepository;
         Customer customer;
         Order order;

        public OrderStepDefinitions()
        {
            var orders = new List<Order>
            {
                new Order {Id = 1, ProductId = 1, Quantity = 1, OrderDate = DateTime.Now}
            };
            mockOrderRepository = new Mock<IRepository<Order>>();

            mockOrderRepository.Setup(x => x.GetAll()).Returns(orders);

                /*        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
                */
        }

        [Given("A user is created")]
        public void GivenUserCreated()
        {
            //customer = customerRepository.CreateCustomer()
            throw new PendingStepException();
        }

        [Given(@"the user has (.*) credits")]
        public void GivenTheUserHasCredits(int p0)
        {
            //customer.Credits = p0;
            throw new PendingStepException();
        }

        [Given(@"the items total out to (.*)")]
        public void GivenTheItemsTotalOutTo(int p0)
        {
            //order.price = p0;
            throw new PendingStepException();
        }

        [When(@"the order is placed")]
        public void WhenTheOrderIsPlaced()
        {
            // order.CheckOrder();
            throw new PendingStepException();
        }

        [Then(@"the result is (.*)")]
        public void ThenTheResultIs(bool result)
        {
            // Assert.Equal(order.status, result)
            throw new PendingStepException();
        }



    }

}
