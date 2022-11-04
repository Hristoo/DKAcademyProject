using System.Net.Sockets;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ThirtStore.Models.Models.MediatR;
using ThirtStore.Models.Models.Requests;

namespace TshirtStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly ILogger<ClientController> _logger;
        private readonly IMediator _mediator;

        public ClientController(IMediator mediator, ILogger<ClientController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost(nameof(AddClient))]
        public async Task<IActionResult> AddClient([FromBody] ClientRequest clientRequest)
        {
            var result = await _mediator.Send(new AddClientCommand(clientRequest));

            if (result == null)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut(nameof(UpdateClient))]
        public async Task<IActionResult> UpdateClient(ClientRequest clientRequest)
        {
            var result = await _mediator.Send(new UpdateClientCommand(clientRequest));

            if (result.HttpStatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteClient))]
        public async Task<IActionResult> DeleteClient(ClientRequest clientRequest)
        {
            var result = await _mediator.Send(new DeleteClientCommand(clientRequest));

            if (result.HttpStatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
