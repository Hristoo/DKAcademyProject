using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ThirtStore.Models.Models;
using ThirtStore.Models.Models.Configurations;
using TshirtStore.DL.Interfaces;

namespace TshirtStore.DL.Repositories.MondoDB
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly MongoClient _mongoClient;
        private IMongoDatabase _database;
        private readonly IMongoCollection<ShoppingCart> _shoppingCartCollection;
        private readonly IOptionsMonitor<MongoDbConfiguration> _mongoDbConfiguration;

        public ShoppingCartRepository(IOptionsMonitor<MongoDbConfiguration> mongoDbConfiguration)
        {
            _mongoDbConfiguration = mongoDbConfiguration;
            _mongoClient = new MongoClient(_mongoDbConfiguration.CurrentValue.ConnectionString);
            _database = _mongoClient.GetDatabase(_mongoDbConfiguration.CurrentValue.DatabaseName);
            _shoppingCartCollection = _database.GetCollection<ShoppingCart>(_mongoDbConfiguration.CurrentValue.DataBaseCollection);
        }

        public async Task AddCart(ShoppingCart cart)
        {
                await _shoppingCartCollection.InsertOneAsync(cart);
        }

        public async Task UpdateCart(ShoppingCart cart)
        {
            await _shoppingCartCollection.ReplaceOneAsync(x => x.ClientId == cart.ClientId, cart);
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
