using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ThirtStore.Models.Models;
using ThirtStore.Models.Models.Configurations;
using TshirtStore.DL.Interfaces;

namespace TshirtStore.DL.Repositories.MondoDB
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ShoppingCart _shoppingCart;
        private readonly MongoClient _mongoClient;
        private IMongoDatabase _database;
        private readonly IMongoCollection<ShoppingCart> _shoppingCartCollection;
        private readonly IOptionsMonitor<MongoDbConfiguration> _mongoDbConfiguration;

        public ShoppingCartRepository(IOptionsMonitor<MongoDbConfiguration> mongoDbConfiguration)
        {
            _shoppingCart = new ShoppingCart()
            {
                Tshirts = new List<Tshirt>()
            };
            _mongoDbConfiguration = mongoDbConfiguration;
            _mongoClient = new MongoClient(_mongoDbConfiguration.CurrentValue.ConnectionString);
            _database = _mongoClient.GetDatabase(_mongoDbConfiguration.CurrentValue.DatabaseName);
            _shoppingCartCollection = _database.GetCollection<ShoppingCart>(_mongoDbConfiguration.CurrentValue.DataBaseCollection);
        }

        public async Task AddToCart(Tshirt tshirt, int clientId)
        {
            _shoppingCart.Tshirts.Add(tshirt);
            _shoppingCart.ClientId = clientId;
            var clientCart = await GetContent(_shoppingCart.ClientId);

            if (clientCart == null)
            {
                await _shoppingCartCollection.InsertOneAsync(_shoppingCart);
            }
            else
            {
                clientCart.Tshirts.Add(tshirt);
                await _shoppingCartCollection.ReplaceOneAsync(x => x.ClientId == _shoppingCart.ClientId, clientCart);
            }
        }

        public async Task EmptyCart(Guid id)
        {
            var deleteCart = await _shoppingCartCollection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<ShoppingCart> GetContent(int clientId)
        {
            var test = await _shoppingCartCollection.FindAsync(x => x.ClientId == clientId);
            return await test.FirstOrDefaultAsync();
        }

        public async Task RemoveFromCart(Tshirt tshirt, int clientId)
        {
            var clientCart = await GetContent(clientId);
            var delTshirt = clientCart.Tshirts.FirstOrDefault(x => x.Id == tshirt.Id);
            clientCart.Tshirts.Remove(delTshirt);

            await _shoppingCartCollection.ReplaceOneAsync(x => x.Id == clientCart.Id, clientCart);
        }
    }
}
