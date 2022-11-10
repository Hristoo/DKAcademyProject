using ThirtStore.Models.Models;

namespace TshirtStore.BL.Interfaces
{
    public interface IShoppingCartService
    {
        Task<ShoppingCart> GetContent(int clientId);

        Task AddToCart(Tshirt tshirt, int clientId );

        Task RemoveFromCart(Tshirt tshirt, int clientId);

        Task EmptyCart(Guid id);
    }
}
