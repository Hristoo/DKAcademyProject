using ThirtStore.Models.Models;

namespace TshirtStore.DL.Interfaces
{
    public interface IShoppingCartRepository
    {
        Task<IEnumerable<ShoppingCart>> GetContent(int clientId);

        Task<ShoppingCart?> AddToCart(Tshirt tshirt);

        Task<ShoppingCart?> RemoveFromCart(Tshirt tshirt);

        Task<ShoppingCart?> EmptyCart();

        Task<ShoppingCart?> FinishOrder(Tshirt tshirt);
    }
}
