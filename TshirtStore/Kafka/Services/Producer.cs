using Confluent.Kafka;
using Kafka.Serializators;
using Microsoft.Extensions.Options;

namespace Kafka.Services
{
    public class Producer<TKey, TValue>
    {
        private readonly IOptionsMonitor<KafkaSettings> _settings;
        private readonly IProducer<TKey, TValue> _producer;

        public Producer(IOptionsMonitor<KafkaSettings> settings)
        {
            _settings = settings;

            var config = new ProducerConfig()
            {
                BootstrapServers = _settings.CurrentValue.BootstrapServers,
            };

            _producer = new ProducerBuilder<TKey, TValue>(config)
                .SetKeySerializer(new KafkaCustomSerializator<TKey>())
                .SetValueSerializer(new KafkaCustomSerializator<TValue>())
                .Build();
        }

        public async Task SendMessage(TKey key, TValue value)
        {
            try
            {
                var msg = new Message<TKey, TValue>()
                {
                    Key = key,
                    Value = value
                };

                var result = await _producer.ProduceAsync(_settings.CurrentValue.Topic, msg);

                if (result != null)
                {
                    Console.WriteLine($"Delivered message: {result.Value} to {result.TopicPartitionOffset}");
                }

            }
            catch (ProduceException<TKey, TValue> e)
            {
                Console.WriteLine($"Delivery message failed: {e.Error.Reason}");
            }
        }
    }
}
