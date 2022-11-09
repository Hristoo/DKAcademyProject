using ThirtStore.Models.Models;

namespace TshirtStore.BL.Interfaces
{
    public interface IShoppingCartService
    {
        Task<IEnumerable<ShoppingCart>> GetContent(int clientId);

        Task AddToCart(Tshirt tshirt);

        Task RemoveFromCart(Tshirt tshirt);

        Task EmptyCart();

        Task<ShoppingCart> FinishOrder(Tshirt tshirt);
    }
}
