namespace ThirtStore.Models.Models.Requests
{
    public class OrderRequest
    {
        public int ClientId { get; set; }

        public DateTime LastUpdated { get; set; }

        public decimal Sum { get; set; }
    }
}
