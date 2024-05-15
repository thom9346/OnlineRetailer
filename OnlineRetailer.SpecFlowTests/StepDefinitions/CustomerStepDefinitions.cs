using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRetailer.SpecFlowTests.StepDefinitions
{
    [Binding]
    public sealed class CustomerStepDefinitions
    {

        [Given(@"A repository for customers is created")]
        public void GivenARepositoryForCustomersIsCreated()
        {
            throw new PendingStepException();
        }

        [When(@"A new customer is created")]
        public void WhenANewCustomerIsCreated()
        {
            throw new PendingStepException();
        }

        [Then(@"The customer is inserted into the repository")]
        public void ThenTheCustomerIsInsertedIntoTheRepository()
        {
            throw new PendingStepException();
        }


    }
}
