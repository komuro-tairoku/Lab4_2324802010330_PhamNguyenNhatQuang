using Microsoft.AspNetCore.Mvc;
using Messaging.Common.Events;
using Messaging.Common.Publishing;

namespace OrderService.API.Controllers
{
    [ApiController]
    [Route("api/order")]
    public class OrderController : ControllerBase
    {
        private readonly Publisher _publisher;

        public OrderController(Publisher publisher)
        {
            _publisher = publisher;
        }

        [HttpPost]
        public IActionResult CreateOrder()
        {
            var orderEvent = new OrderPlacedEvent
            {
                OrderId = Guid.NewGuid()
            };

            _publisher.Publish(
                "ecommerce_exchange",
                "order.placed",
                orderEvent
            );

            return Ok("Order event sent to RabbitMQ");
        }
    }
}