using ThirtStore.Models.Models;
using TshirtStore.BL.Interfaces;
using TshirtStore.DL.Interfaces;
using TshirtStore.DL.Repositories.MondoDB;

namespace TshirtStore.BL.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        public async Task AddToCart(Tshirt tshirt, int clientId)
        {
            await _shoppingCartRepository.AddToCart(tshirt, clientId);
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
