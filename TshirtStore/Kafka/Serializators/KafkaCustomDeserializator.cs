using Confluent.Kafka;
using MessagePack;

namespace Kafka.Serializators
{
    public class KafkaCustomDeserializator<T> : IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            return MessagePackSerializer.Deserialize<T>(data.ToArray());
        }
    }
}
