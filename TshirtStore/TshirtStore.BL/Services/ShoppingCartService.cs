using ThirtStore.Models.Models;
using TshirtStore.BL.Interfaces;
using TshirtStore.DL.Interfaces;
using TshirtStore.DL.Repositories.MondoDB;

namespace TshirtStore.BL.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly ShoppingCart _shoppingCart;

        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        public async Task AddToCart(Tshirt tshirt, int clientId)
        {
            var cart = await _shoppingCartRepository.GetContent(clientId);

            if (cart == null)
            {
                cart = new ShoppingCart() { Tshirts = new List<Tshirt>()};
                cart.Tshirts.Add(tshirt);
                cart.ClientId = clientId;
                await _shoppingCartRepository.AddCart(cart);
            }
            else
            {
                var existTshirt = cart.Tshirts.FirstOrDefault(x => x.Id == tshirt.Id);

                if (existTshirt != null)
                {
                    existTshirt.Quantity += tshirt.Quantity;
                }
                else
                {
                    cart.Tshirts.Add(tshirt);
                }
                await _shoppingCartRepository.UpdateCart(cart);
            }
        }

        public async Task EmptyCart(Guid id)
        {
           await _shoppingCartRepository.EmptyCart(id);
        }

        public Task<ShoppingCart> GetContent(int clientId)
        {
            return _shoppingCartRepository.GetContent(clientId);
        }

        public async Task RemoveFromCart(Tshirt tshirt, int clientId)
        {
            await _shoppingCartRepository.RemoveFromCart(tshirt, clientId);
        }
    }
}
