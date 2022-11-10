using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ThirtStore.Models.Models
{
    public class ShoppingCart
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        public int ClientId { get; set; }

        public List<Tshirt> Tshirts { get; set; }
    }
}
