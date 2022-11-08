using Confluent.Kafka;
using MessagePack;

namespace Kafka.Serializators
{
    public class KafkaCustomSerializator<T> : ISerializer<T>
    {
        public byte[] Serialize(T data, SerializationContext context)
        {
            return MessagePackSerializer.Serialize(data);
        }
    }
}
