using System.Net;
using Microsoft.AspNetCore.Mvc;
using ThirtStore.Models.Models;
using ThirtStore.Models.Models.Requests;
using TshirtStore.BL.Interfaces;

namespace TshirtStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ThsirtController : ControllerBase
    {
        private readonly ITshirtService _tshirtService;
        private readonly ILogger<ThsirtController> _logger;

        public ThsirtController(ITshirtService tshirtService, ILogger<ThsirtController> logger)
        {
            _tshirtService = tshirtService;
            _logger = logger;
        }

        [HttpGet(nameof(GetAllThsirts))]
        public async Task<IActionResult> GetAllThsirts()
        {
            return Ok( await _tshirtService.GetAllTshirts());
        }

        [HttpPost(nameof(AddTshirt))]
        public async Task<IActionResult?> AddTshirt(TshirtRequest tshirt)
        {
            var result = await _tshirtService.AddThirt(tshirt);

            return Ok(result);
        }

        [HttpPut(nameof(UpdateTshirt))]
        public async Task<Tshirt?> UpdateTshirt(Tshirt tshirt)
        {
            var result = await _tshirtService.UpdateThirt(tshirt);
            return result;
        }

        [HttpDelete(nameof(DeleteTshirt))]
        public async Task<IActionResult?> DeleteTshirt(int tshirtId)
        {
            var result = await _tshirtService.DeleteThirt(tshirtId);
            return Ok(result);
        }


    }
}
