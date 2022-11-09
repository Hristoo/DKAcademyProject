using Microsoft.AspNetCore.Mvc;
using ThirtStore.Models.Models;
using TshirtStore.BL.Interfaces;

namespace TshirtStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpPost(nameof(AddToCart))]
        public async Task<IActionResult> AddToCart(Tshirt tshirt)
        {
            return Ok(_shoppingCartService.AddToCart(tshirt));
        }

        [HttpGet(nameof(GetContent))]
        public async Task<IActionResult> GetContent(int clientId)
        {
            return Ok(_shoppingCartService.GetContent(clientId));
        }
    }
}
