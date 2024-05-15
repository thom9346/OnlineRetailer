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

        [Given(@"A product is created")]
        public void GivenAProductIsCreated()
        {
            throw new PendingStepException();
        }

        [Given(@"There are (.*) items available")]
        public void GivenThereAreItemsAvailable(int p0)
        {
            throw new PendingStepException();
        }

        [When(@"the status is checked")]
        public void WhenTheStatusIsChecked()
        {
            throw new PendingStepException();
        }

        [Then(@"the availability is (.*)")]
        public void ThenTheAvailabilityIsTrue(bool result)
        {
            throw new PendingStepException();
        }


    }
}
