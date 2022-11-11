using Microsoft.AspNetCore.Mvc;
using ThirtStore.Models.Models;
using ThirtStore.Models.Models.Requests;
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
        public async Task<IActionResult> AddToCart(ShoppingCartRequest cart)
        {
            return Ok(await _shoppingCartService.AddToCart(cart));
        }

        [HttpGet(nameof(GetContent))]
        public async Task<IActionResult> GetContent(int clientId)
        {
            return Ok(await _shoppingCartService.GetContent(clientId));
        }

        [HttpDelete(nameof(EmptyCart))]
        public async Task<IActionResult> EmptyCart(Guid id)
        {
            await _shoppingCartService.EmptyCart(id);
            return Ok();
        }

        [HttpPut(nameof(RemoveFromCart))]
        public async Task<IActionResult> RemoveFromCart(Tshirt tshirt, int clientId)
        {
            await _shoppingCartService.RemoveFromCart(tshirt, clientId);
            return Ok();
        }
    }
}
