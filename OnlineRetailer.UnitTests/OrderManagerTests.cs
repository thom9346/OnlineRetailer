using Moq;
using OnlineRetailer.Core;
using OnlineRetailer.Core.Entities;
using OnlineRetailer.Core.Interfaces;
using OnlineRetailer.Core.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace OnlineRetailer.UnitTests
{
    public class OrderManagerTests
    {
        private readonly IOrderManager _orderManager;

        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IRepository<Order>> _mockOrderRepository;
        private readonly Mock<IRepository<Product>> _mockProductRepository;
        private readonly Mock<IRepository<Customer>> _mockCustomerRepository;

        private readonly List<Product> _products;
        private readonly List<Order> _orders;
        private readonly List<OrderLine> _orderLines;
        private readonly List<Customer> _customers;

        public OrderManagerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockOrderRepository = new Mock<IRepository<Order>>();
            _mockProductRepository = new Mock<IRepository<Product>>();
            _mockCustomerRepository = new Mock<IRepository<Customer>>();

            _products = new List<Product>
            {
                new Product { Id = 1, Name = "Product A", Price = 100, ItemsInStock = 20 },
                new Product { Id = 2, Name = "Product B", Price = 200, ItemsInStock = 2 }
            };
            _mockProductRepository.Setup(repo => repo.GetAll()).Returns(_products);
            _mockProductRepository.Setup(repo => repo.Get(It.IsAny<int>()))
                .Returns((int id) => _products.Find(p => p.Id == id));

            _orderLines = new List<OrderLine>
            {
                new OrderLine { Id = 1, ProductId = 1, Quantity = 3 },
                new OrderLine { Id = 2, ProductId = 2, Quantity = 1 },
            };

            _orders = new List<Order>
            {
                new Order { Id = 1, CustomerId = 1, OrderDate = DateTime.Today, OrderLines = _orderLines },
                new Order { Id = 2, CustomerId = 2, OrderDate = DateTime.Today, OrderLines = _orderLines }
            };

            _customers = new List<Customer>
            {
                new Customer { Id = 1, Balance = 1000 },
                new Customer { Id = 2, Balance = 100 }
            };

            _mockCustomerRepository.Setup(repo => repo.Get(It.IsAny<int>()))
                .Returns((int id) => _customers.Find(c => c.Id == id));

            _mockUnitOfWork.Setup(uow => uow.Orders).Returns(_mockOrderRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.Products).Returns(_mockProductRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.Customers).Returns(_mockCustomerRepository.Object);

            _orderManager = new OrderManager(_mockUnitOfWork.Object);
        }

        [Fact]
        public void CalculateTotalCost_ProductInStock_ReturnsTotalCost()
        {
            var order = new Order
            {
                OrderLines = new List<OrderLine> { new OrderLine { ProductId = 1, Quantity = 2 } }
            };

            var totalCost = _orderManager.CalculateTotalCost(order);

            Assert.Equal(200, totalCost);
        }
        [Fact]
        public void CalculateTotalCost_ProductDoesNotExist_ThrowsException()
        {
            var order = new Order
            {
                OrderLines = new List<OrderLine> { new OrderLine { ProductId = 999, Quantity = 1 } }
            };

            Assert.Throws<InvalidOperationException>(() => _orderManager.CalculateTotalCost(order));
        }
        [Fact]
        public void CalculateTotalCost_ProductOutOfStock_ThrowsException()
        {
            var order = new Order
            {
                OrderLines = new List<OrderLine> { new OrderLine { ProductId = 2, Quantity = 100 } }
            };

            Assert.Throws<InvalidOperationException>(() => _orderManager.CalculateTotalCost(order));
        }
        [Fact]
        public void CalculateTotalCost_EmptyOrder_ReturnsZero()
        {
            var order = new Order
            {
                OrderLines = new List<OrderLine>()
            };

            var totalCost = _orderManager.CalculateTotalCost(order);

            Assert.Equal(0, totalCost);
        }
        [Fact]
        public void CalculateTotalCost_MultipleProductsInStock_ReturnsTotalCost()
        {
            var order = new Order
            {
                OrderLines = new List<OrderLine>
                {
                    new OrderLine { ProductId = 1, Quantity = 5 },
                    new OrderLine { ProductId = 2, Quantity = 2 }
                }
            };
            var totalCost = _orderManager.CalculateTotalCost(order);

            Assert.Equal(900, totalCost);
        }

        [Fact]
        public void CreateOrder_ProductDoesNotExist_ReturnsFalse()
        {
            var order = new Order
            {
                CustomerId = 1,
                OrderLines = new List<OrderLine> { new OrderLine { ProductId = 3, Quantity = 10 } }
            };

            var result = _orderManager.CreateOrder(order);

            Assert.False(result);
        }

        [Fact]
        public void CreateOrder_NotEnoughStock_ReturnsFalse()
        {
            var order = new Order
            {
                CustomerId = 1,
                OrderLines = new List<OrderLine> { new OrderLine { ProductId = 2, Quantity = 10 } }
            };

            var result = _orderManager.CreateOrder(order);

            Assert.False(result);
        }

        [Fact]
        public void CreateOrder_EnoughStock_ReturnsTrue()
        {
            var order = new Order
            {
                CustomerId = 1,
                OrderLines = new List<OrderLine> { new OrderLine { ProductId = 1, Quantity = 10 } }
            };

            var result = _orderManager.CreateOrder(order);
            Assert.True(result);
        }
        [Fact]
        public void CreateOrder_CustomerDoesNotExist_ReturnsFalse()
        {
            var order = new Order
            {
                CustomerId = 3,
                OrderLines = new List<OrderLine> { new OrderLine { ProductId = 1, Quantity = 10 } }
            };

            var result = _orderManager.CreateOrder(order);

            Assert.False(result);
        }

        [Fact]
        public void CreateOrder_CustomerHasInsufficientBalance_ReturnsFalse()
        {
            var order = new Order
            {
                CustomerId = 2,
                OrderLines = new List<OrderLine> { new OrderLine { ProductId = 1, Quantity = 10 } }
            };

            var result = _orderManager.CreateOrder(order);

            Assert.False(result);
        }

        [Fact]
        public void CreateOrder_OrderIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _orderManager.CreateOrder(null));
        }

        [Fact]
        public void CreateOrder_OrderHasNoOrderLines_ThrowsArgumentException()
        {
            var order = new Order
            {
                CustomerId = 1,
                OrderLines = new List<OrderLine>()
            };

            Assert.Throws<ArgumentException>(() => _orderManager.CreateOrder(order));
        }

        [Fact]
        public void CreateOrder_SuccessfulOrderCreationWithMultipleOrderLines_ReturnsTrue()
        {
            var order = new Order
            {
                CustomerId = 1,
                OrderLines = new List<OrderLine>
                {
                    new OrderLine { ProductId = 1, Quantity = 2 },
                    new OrderLine { ProductId = 2, Quantity = 1 }
                }
            };

            var result = _orderManager.CreateOrder(order);

            Assert.True(result);
        }
    }
}