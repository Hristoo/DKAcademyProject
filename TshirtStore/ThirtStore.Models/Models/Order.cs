using MessagePack;

namespace ThirtStore.Models.Models
{
    [MessagePackObject]
    public class Order
    {
        [Key(0)]
        public int Id { get; set; }
        [Key(1)]
        public int ClientId { get; set; }
        [Key(2)]
        public DateTime LastUpdated { get; set; }
        [Key(3)]
        public decimal Sum { get; set; }
    }
}
