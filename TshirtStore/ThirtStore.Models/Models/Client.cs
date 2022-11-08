using MessagePack;

namespace ThirtStore.Models.Models
{
    [MessagePackObject]
    public class Client
    {
        [Key(0)]
        public int Id { get; set; }
        [Key(1)]
        public string Name { get; set; }
        [Key(2)]
        public string Address { get; set; }
    }
}
