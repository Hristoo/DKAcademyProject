namespace ThirtStore.Models.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public DateTime LastUpdated { get; set; }

        public decimal Sum { get; set; }
    }
}
