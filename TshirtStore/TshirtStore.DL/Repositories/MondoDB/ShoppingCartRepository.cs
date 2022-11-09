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

        public async Task<ShoppingCart?> AddToCart(Tshirt tshirt)
        {
            _shoppingCart.Tshirts.Add(tshirt);
            _shoppingCart.ClientId = 1;
            await _shoppingCartCollection.InsertOneAsync(_shoppingCart);
            return _shoppingCart;
        }

        public Task<ShoppingCart?> EmptyCart()
        {
            throw new NotImplementedException();
        }

        public Task<ShoppingCart?> FinishOrder(Tshirt tshirt)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ShoppingCart>> GetContent(int clientId)
        {
            return (await _shoppingCartCollection.FindAsync(x => x.ClientId == clientId)).ToList();
        }

        public Task<ShoppingCart?> RemoveFromCart(Tshirt tshirt)
        {
            throw new NotImplementedException();
        }
    }
}
