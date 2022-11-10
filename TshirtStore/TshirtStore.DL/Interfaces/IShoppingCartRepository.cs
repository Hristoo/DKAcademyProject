using ThirtStore.Models.Models;

namespace TshirtStore.DL.Interfaces
{
    public interface IShoppingCartRepository
    {
        Task<ShoppingCart> GetContent(int clientId);

        Task AddToCart(Tshirt tshirt, int clientId);

        Task RemoveFromCart(Tshirt tshirt, int clientId);

        Task EmptyCart(Guid id);
    }
}
