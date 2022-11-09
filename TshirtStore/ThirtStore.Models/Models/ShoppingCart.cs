namespace ThirtStore.Models.Models
{
    public class ShoppingCart
    {
        public Guid Id { get; set; }

        public int ClientId { get; set; }

        public List<Tshirt> Tshirts { get; set; }
    }
}
