using OnlineRetailer.Core.Entities;
using OnlineRetailer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRetailer.Core.Services
{
    public class OrderManager : IOrderManager
    {
        private readonly IRepository<Order> orderRepository;
        private readonly IRepository<Product> productRepository;

        public OrderManager(IRepository<Order> orderRepo, IRepository<Product> productRepo)
        {
            orderRepository = orderRepo;
            productRepository = productRepo;
        }

        public bool CreateOrder(Order order)
        {
            var product = productRepository.Get(order.ProductId);
            //if product doesnt exist or not enough stoc´k
            if (product == null || product.ItemsInStock < order.Quantity)
            {
                return false;
            }

            product.ItemsInStock -= order.Quantity;
            productRepository.Edit(product);

            orderRepository.Add(order);
            return true;
        }

        public bool UpdateOrder(Order order)
        {
            var existingOrder = orderRepository.Get(order.Id);
            if (existingOrder == null)
            {
                return false;
            }

            existingOrder.Quantity = order.Quantity;
            existingOrder.CustomerId = order.CustomerId;
            existingOrder.ProductId = order.ProductId;

            existingOrder.OrderDate = order.OrderDate;

            orderRepository.Edit(existingOrder);
            return true;
        }

        public bool CheckAvailability(int productId, int quantity)
        {
            var product = productRepository.Get(productId);
            if (product == null || product.ItemsInStock < quantity)
            {
                return false;
            }

            return true;
        }
    }
}

