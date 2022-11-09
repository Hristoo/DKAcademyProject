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

        public  Task AddToCart(Tshirt tshirt)
        {
            return  _shoppingCartRepository.AddToCart(tshirt);
        }

        public  Task EmptyCart()
        {
           return _shoppingCartRepository.EmptyCart();
        }

        public Task<ShoppingCart> FinishOrder(Tshirt tshirt)
        {
            return _shoppingCartRepository.FinishOrder(tshirt);
        }

        public Task<IEnumerable<ShoppingCart>> GetContent(int clientId)
        {
            return _shoppingCartRepository.GetContent(clientId);
        }

        public Task RemoveFromCart(Tshirt tshirt)
        {
            return _shoppingCartRepository.RemoveFromCart(tshirt);
        }
    }
}
