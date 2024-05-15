using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRetailer.SpecFlowTests.StepDefinitions
{
    [Binding]
    public sealed class OrderStepDefinitions
    {

        // Customer customer;
        // Order order;

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
