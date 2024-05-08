using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OnlineRetailer.Core;
using OnlineRetailer.Core.Entities;



namespace OnlineRetailer.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : Controller
    {
        private readonly IRepository<Order> repository;

        public OrdersController(IRepository<Order> repos)
        {
            repository = repos;
        }

        // GET: orders
        [HttpGet(Name = "GetOrders")]
        public IEnumerable<Order> Get()
        {
            return repository.GetAll();
        }
        // GET order/5
        [HttpGet("{id}", Name = "GetOrder")]
        public IActionResult Get(int id)
        {
            var item = repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        // POST bookings
        [HttpPost]
        public IActionResult Post([FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest();
            }

            //bool created = bookingManager.CreateBooking(booking);

            //if (created)
            //{
            //    return CreatedAtRoute("GetBookings", null);
            //}
            //else
            //{
            //    return Conflict("The booking could not be created. All rooms are occupied. Please try another period.");
            //}

            repository.Add(order);
            return CreatedAtRoute("GetOrders", null);
        }

        // PUT bookings/5
        //to change quantity for now
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Order order)
        {
            if (order == null || order.Id != id)
            {
                return BadRequest();
            }

            var modifiedOrder = repository.Get(id);

            if (modifiedOrder == null)
            {
                return NotFound();
            }

            // This implementation will only modify the booking's state and customer.
            // It is not safe to directly modify StartDate, EndDate and Room, because
            // it could conflict with other active bookings.
            modifiedOrder.Quantity = order.Quantity;
            modifiedOrder.CustomerId = order.CustomerId;

            repository.Edit(modifiedOrder);
            return NoContent();
        }

        // DELETE bookings/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (repository.Get(id) == null)
            {
                return NotFound();
            }

            repository.Remove(id);
            return NoContent();
        }

    }
}
