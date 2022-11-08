using MessagePack;

namespace ThirtStore.Models.Models
{
    [MessagePackObject]
    public class Tshirt
    {
        [Key(0)]
        public int Id { get; set; }
        [Key(1)]
        public string Name { get; set; }
        [Key(2)]
        public string Size { get; set; }
        [Key(3)]
        public string Color { get; set; }
        [Key(4)]
        public decimal Price { get; set; }
        [Key(5)]
        public int Quantity { get; set; }

    }
}