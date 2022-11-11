using ThirtStore.Models.Models;
using ThirtStore.Models.Models.Requests;
using ThirtStore.Models.Models.Responses;
using TshirtStore.BL.Interfaces;
using TshirtStore.DL.Interfaces;

namespace TshirtStore.BL.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly ITshirtRepository _tshirtRepository;
        private readonly ShoppingCart _shoppingCart;

        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository, ITshirtRepository tshirtRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _tshirtRepository = tshirtRepository;
        }

        public async Task<ShoppingCartResponse> AddToCart(ShoppingCartRequest cartRequest)
        {
            var cart = await _shoppingCartRepository.GetContent(cartRequest.ClientId);

            if (cart == null)
            {
                cart = new ShoppingCart()
                {
                    ClientId = cartRequest.ClientId,
                    Tshirts = cartRequest.Tshirts,
                    
                };
                await _shoppingCartRepository.AddCart(cart);

            }
            else
            {
                foreach (var tshirt in cartRequest.Tshirts)
                {
                    var availableTshirt = await _tshirtRepository.GetTshirtsById(tshirt.Id);

                    if (availableTshirt == null)
                    {
                        return new ShoppingCartResponse()
                        {
                            HttpStatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = $"Invalid tshirt!",
                            ShoppingCart = cart
                            
                        };
                    }

                    if (availableTshirt.Quantity < tshirt.Quantity)
                    {
                        return new ShoppingCartResponse()
                        {
                            HttpStatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = $"No enough quantity {tshirt.Name} tshirts! Try again later!",
                            ShoppingCart = cart
                        };
                    }

                    var existTshirt = cart.Tshirts.FirstOrDefault(x => x.Id == tshirt.Id);

                    if (existTshirt != null)
                    {
                        existTshirt.Quantity += tshirt.Quantity;
                    }
                    else
                    {
                        cart.Tshirts.Add(tshirt);
                    }
                }
                await _shoppingCartRepository.UpdateCart(cart);
            }

            return new ShoppingCartResponse()
            {
                HttpStatusCode = System.Net.HttpStatusCode.BadRequest,
                Message = $"Tshirt was added in shopping cart successfully!",
                ShoppingCart = cart
            };
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
