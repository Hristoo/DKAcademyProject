using ThirtStore.Models.Models;

namespace TshirtStore.DL.Interfaces
{
    public interface IShoppingCartRepository
    {
        Task<ShoppingCart> GetContent(int clientId);

        Task AddCart(ShoppingCart cart);

        Task UpdateCart(ShoppingCart newCart);

        Task RemoveFromCart(Tshirt tshirt, int clientId);

        Task EmptyCart(Guid id);
    }
}
