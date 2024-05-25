﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OnlineRetailer.Core;
using OnlineRetailer.Core.Entities;
using OnlineRetailer.Infrastructure;

namespace OnlineRetailer.Infrastructure.Repositories
{
    public class OrderRepository : IRepository<Order>
    {
        private readonly OnlineRetailerContext db;

        public OrderRepository(OnlineRetailerContext context)
        {
            db = context;
        }

        public void Add(Order entity)
        {
            db.Order.Add(entity);
        }

        public void Edit(Order entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public Order Get(int id)
        {
            return db.Order.Include(o => o.OrderLines).SingleOrDefault(o => o.Id == id);
        }

        public IEnumerable<Order> GetAll()
        {
            return db.Order.Include(o => o.OrderLines).ToList();
        }

        public void Remove(int id)
        {
            var entity = db.Customer.Find(id);
            if (entity != null)
            {
                db.Customer.Remove(entity);
            }
        }
    }
}
