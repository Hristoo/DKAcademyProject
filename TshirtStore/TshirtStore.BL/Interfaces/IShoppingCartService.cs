using ThirtStore.Models.Models;
using ThirtStore.Models.Models.Requests;
using ThirtStore.Models.Models.Responses;

namespace TshirtStore.BL.Interfaces
{
    public interface IShoppingCartService
    {
        Task<ShoppingCart> GetContent(int clientId);

        Task<ShoppingCartResponse> AddToCart(ShoppingCartRequest cart);

        Task RemoveFromCart(Tshirt tshirt, int clientId);

        Task EmptyCart(Guid id);
    }
}
