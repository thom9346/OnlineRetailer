using Castle.Core.Resource;
using Moq;
using OnlineRetailer.Core;
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

        private Mock<IRepository<Customer>> mockCustomerRepository;

        List<Customer> customers;

        List<Customer> result;

        public CustomerStepDefinitions() {


 
        }

        [Given(@"A repository for customers is created")]
        public void GivenARepositoryForCustomersIsCreated()
        {
            customers = new List<Customer>
            {
                new Customer {Balance = 500, Email = "Emil@gmail.com",Id = 1, Name = "Emil"}
            };

            mockCustomerRepository = new Mock<IRepository<Customer>>();

            mockCustomerRepository.Setup(x => x.GetAll()).Returns(customers);
        }

        [When(@"A new customer is created")]
        public void WhenANewCustomerIsCreated()
        {
            customers.Add(new Customer { Balance = 200, Email = "LarsLarsen@gmail.com", Id = 2, Name = "Lars" });
            mockCustomerRepository.Object.Add(customers[1]);
            result = (List<Customer>)mockCustomerRepository.Object.GetAll();
        }

        [Then(@"The customer is inserted into the repository")]
        public void ThenTheCustomerIsInsertedIntoTheRepository()
        {
            Assert.Equal(result[1], customers[1]); 
        }


    }
}
