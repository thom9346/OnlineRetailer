using System;
using System.Collections.Generic;
using System.Linq;
using OnlineRetailer.Core;
using OnlineRetailer.Core.Entities;
using OnlineRetailer.Infrastructure;

namespace OnlineRetailer.Infrastructure.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly OnlineRetailerContext db;

        public ProductRepository(OnlineRetailerContext context)
        {
            db = context;
        }

        public void Add(Product entity)
        {
            throw new NotImplementedException();
        }

        public void Edit(Product entity)
        {
            throw new NotImplementedException();
        }

        public Product Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetAll()
        {
            return db.Product.ToList();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
