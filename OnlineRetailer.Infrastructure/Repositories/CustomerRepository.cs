using System;
using System.Collections.Generic;
using System.Linq;
using OnlineRetailer.Core;
using OnlineRetailer.Infrastructure;

namespace OnlineRetailer.Infrastructure.Repositories
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly OnlineRetailerContext db;

        public CustomerRepository(OnlineRetailerContext context)
        {
            db = context;
        }

        public void Add(Customer entity)
        {
            throw new NotImplementedException();
        }

        public void Edit(Customer entity)
        {
            throw new NotImplementedException();
        }

        public Customer Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> GetAll()
        {
            return db.Customer.ToList();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
