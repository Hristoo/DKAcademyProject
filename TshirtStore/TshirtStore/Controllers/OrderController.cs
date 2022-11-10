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

        public OrderController(IMediator mediator, ILogger<OrderController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost(nameof(AddOrder))]
        public async Task<IActionResult> AddOrder([FromBody] int clientId)
        {
            var result = await _mediator.Send(new AddOrderCommand(clientId));       

            if (result.Order == null)
            {
                return BadRequest(result);
            }

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

        [HttpPut(nameof(UpdateOrder))]
        public async Task<IActionResult> UpdateOrder([FromBody] Order order)
        {
            var result = await _mediator.Send(new UpdateOrderCommand(order));

            if (result == null)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
