using Kafka.Services;
using Confluent.Kafka;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ThirtStore.Models.Models.MediatR;
using ThirtStore.Models.Models.Requests;
using ThirtStore.Models.Models;

namespace TshirtStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IMediator _mediator;
        private readonly Producer<int, Order> _producer;

        public OrderController(IMediator mediator, ILogger<OrderController> logger, Producer<int, Order> producer)
        {
            _mediator = mediator;
            _logger = logger;
            _producer = producer;
        }

        [HttpPost(nameof(AddOrder))]
        public async Task<IActionResult> AddOrder([FromBody] OrderRequest orderRequest)
        {
            var result = await _mediator.Send(new AddOrderCommand(orderRequest));       

            if (result == null)
            {
                return BadRequest(result);
            }

            await _producer.SendMessage(result.Order.Id, result.Order);

            return Ok(result);
        }


        [HttpDelete(nameof(DeleteOrder))]
        public async Task<IActionResult> DeleteOrder([FromBody] int orderId)
        {
            var result = await _mediator.Send(new DeleteOrderCommand(orderId));

            if (result == null)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet(nameof(GetOrderById))]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var result = await _mediator.Send(new GetOrderByIdCommand(orderId));

            if (result == null)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
