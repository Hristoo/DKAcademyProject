using System.Security.Cryptography;
using System.Threading.Tasks.Dataflow;
using AutoMapper;
using Confluent.Kafka;
using Kafka.Serializators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ThirtStore.Models.Models;
using ThirtStore.Models.Models.Requests;
using TshirtStore.DL.Interfaces;

namespace Kafka.Services
{
    public class Consumer<TKey, TValue> : IHostedService
    {
        private readonly IOptionsMonitor<KafkaSettings> _settings;
        private readonly IConsumer<TKey, TValue> _consumer;
        private readonly IMapper _mapper;
        private readonly IReportSqlRepository _reportSqlRepository;


        public Consumer(IOptionsMonitor<KafkaSettings> settings, IMapper mapper, IReportSqlRepository reportSqlRepository)
        {
            _settings = settings;

            var config = new ConsumerConfig
            {
                BootstrapServers = _settings.CurrentValue.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                GroupId = _settings.CurrentValue.GroupId,
            };

            _consumer = new ConsumerBuilder<TKey, TValue>(config)
                .SetKeyDeserializer(new KafkaCustomDeserializator<TKey>())
                .SetValueDeserializer(new KafkaCustomDeserializator<TValue>())
                .Build();

            _consumer.Subscribe($"{_settings.CurrentValue.Topic}");
            _mapper = mapper;
            _reportSqlRepository = reportSqlRepository;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() =>
            {
                var transformBlock = new TransformBlock<Order, MonthlyReportRequest>(async result =>
                {
                    var currentMonth = DateTime.Now.ToString("MMM");
                    var incomes = await _reportSqlRepository.GetMonthlyIncomes(currentMonth);

                    var report = new MonthlyReportRequest()
                    {
                        Month = currentMonth,
                        Incomes = incomes + result.Sum,
                        Updated = DateTime.UtcNow
                    };

                    if (incomes > 0)
                    {
                        await _reportSqlRepository.UpdateReport(report);
                    }
                    else
                    {
                        await _reportSqlRepository.AddReport(report);
                    }

                    return report;
                });

                var actionBlock = new ActionBlock<MonthlyReportRequest>(report =>
                {
                    Console.WriteLine($"The monthly report is {report.Incomes}");
                });

                transformBlock.LinkTo(actionBlock);

                while (!cancellationToken.IsCancellationRequested)
                {
                    var data = _consumer.Consume();
                    var newOrder = _mapper.Map<Order>(data.Message.Value);

                    transformBlock.Post(newOrder);
                }
            });

            return Task.CompletedTask;
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            _consumer.Dispose();
            return Task.CompletedTask;
        }
    }
}
