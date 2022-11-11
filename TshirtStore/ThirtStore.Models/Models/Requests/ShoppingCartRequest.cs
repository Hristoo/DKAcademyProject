namespace ThirtStore.Models.Models.Requests
{
    public class ShoppingCartRequest
    {
        public int ClientId { get; set; }

        public List<Tshirt> Tshirts { get; set; }
    }
}
