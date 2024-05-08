using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Sos.Application.Core.Abstractions.MessageQueue;
using System.Text.Json;

namespace Sos.Infrastructure.MessageQueue
{
    /// <summary>
    /// Represents the kafka consumer of the message queue.
    /// </summary>
    public sealed class KafkaConsumer : IConsumer
    {
        private readonly ILogger<KafkaConsumer> _logger;
        private readonly string _bootstrapServers;

        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaConsumer"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="bootstrapServers">The bootstrap servers.</param>
        public KafkaConsumer(
            ILogger<KafkaConsumer> logger,
            string bootstrapServers
        )
        {
            _logger = logger;
            _bootstrapServers = bootstrapServers;
        }

        // < inheritdoc />
        public Task<T> SubscribeAsync<T>(string topic, string groupId, CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                AutoOffsetReset = AutoOffsetReset.Earliest,
                BootstrapServers = _bootstrapServers,
                BrokerAddressFamily = BrokerAddressFamily.V4,
                GroupId = groupId,
            };

            try
            {
                using (var consumerBuilder = new ConsumerBuilder<Ignore, string>(config).Build())
                {
                    consumerBuilder.Subscribe(topic);

                    while (true)
                    {
                        var consumer = consumerBuilder.Consume(cancellationToken);

                        _logger.LogInformation(
                            "*** [{DateTime}] Kafka Consumer received: {Message} in topic: {Topic} - partition: {Partition} - offset: {Offset} ***",
                            consumer.Message.Timestamp.UtcDateTime,
                            consumer.Message.Value,
                            topic,
                            consumer.Partition.Value,
                            consumer.Offset!.Value
                         );

                        var result = JsonSerializer.Deserialize<T>(consumer.Message.Value);

                        return Task.FromResult(result)!;
                    }
                }
            }
            catch (ConsumeException ex)
            {
                _logger.LogError($"===> Error occured: {ex.Message}");
                throw;
            }
        }

        // < inheritdoc />
        public Task UnsubscribeAsync(string topic, string groupId, CancellationToken cancellationToken = default)
        {
            var config = new ConsumerConfig
            {
                AutoOffsetReset = AutoOffsetReset.Earliest,
                BootstrapServers = _bootstrapServers,
                BrokerAddressFamily = BrokerAddressFamily.V4,
                GroupId = groupId,
            };

            try
            {
                using (var consumerBuilder = new ConsumerBuilder<Ignore, string>(config).Build())
                {
                    consumerBuilder.Unsubscribe();

                    consumerBuilder.Close();

                    consumerBuilder.Dispose();

                    return Task.CompletedTask;
                }
            }
            catch (ConsumeException ex)
            {
                _logger.LogError($"===> Error occured: {ex.Message}");
                throw;
            }
        }
    }
}
