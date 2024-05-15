using OnlineRetailer.Core.Entities;
using OnlineRetailer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace OnlineRetailer.Core.Services
{
    public class OrderManager : IOrderManager
    {
        private readonly IRepository<Order> orderRepository;
        private readonly IRepository<Product> productRepository;
        private readonly IRepository<Customer> customerRepository;

        public OrderManager(IRepository<Order> orderRepo, IRepository<Product> productRepo, IRepository<Customer> customerRepo)
        {
            orderRepository = orderRepo;
            productRepository = productRepo;
            customerRepository = customerRepo;
        }

        public bool CreateOrder(Order order)
        {
            using (var transaction = new TransactionScope())
            {
                Customer customer = customerRepository.Get(order.CustomerId);

                decimal totalCost = 0;

                foreach (var orderLine in order.OrderLines)
                {
                    Product product = productRepository.Get(orderLine.ProductId);


                    if (CheckAvailability(product, orderLine))
                    {
                        totalCost += orderLine.Quantity * product.Price;
                        product.ItemsInStock -= orderLine.Quantity;
                        productRepository.Edit(product);
                    }
                    else
                    {
                        return false;
                    }
                     
                } 

                if (customer.Balance >= totalCost)
                {
                    orderRepository.Add(order);
                    transaction.Complete();
                    return true;
                } 
                return false;
            }
        }

        public bool UpdateOrder(Order order)
        {
            return false;
        }


        public bool CheckAvailability(Product product, OrderLine orderLine)
        {
            

            if (product == null || product.ItemsInStock < orderLine.Quantity)
            {
                return false;
            }
            return true;
        }
 
 
    }
}

