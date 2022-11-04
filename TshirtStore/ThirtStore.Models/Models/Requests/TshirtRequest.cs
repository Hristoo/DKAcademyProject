namespace ThirtStore.Models.Models.Requests
{
    public class TshirtRequest
    {
        public string Size { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
