using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Sos.Application.Core.Abstractions.MessageQueue;
using System.Net;

namespace Sos.Infrastructure.MessageQueue
{
    /// <summary>
    /// Represents the kafka producer of the message queue.
    /// </summary>
    public sealed class KafkaProducer : IProducer
    {
        private readonly ILogger<KafkaProducer> _logger;
        private readonly string _bootstrapServers;

        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaProducer"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="bootstrapServers">The bootstrap servers.</param>
        public KafkaProducer(
            ILogger<KafkaProducer> logger,
            string bootstrapServers
        )
        {
            _logger = logger;
            _bootstrapServers = bootstrapServers;
        }

        // < inheritdoc />
        public async Task PublishAsync(string topic, object message)
        {
            var config = new ProducerConfig
            {
                ClientId = Dns.GetHostName(),
                BootstrapServers = _bootstrapServers,
                BrokerAddressFamily = BrokerAddressFamily.V4,
            };

            try
            {
                using (var producerBuilder = new ProducerBuilder<Null, string>(config).Build())
                {
                    var messageValue = new Message<Null, string>
                    {
                        //Value = JsonSerializer.Serialize(message, new JsonSerializerOptions
                        //{
                        //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        //}),
                        Value = message.ToString()!,
                    };

                    var result = await producerBuilder.ProduceAsync(
                        topic,
                        messageValue
                    );

                    _logger.LogInformation(
                        "*** [{DateTime}] Kafka Producer sent: {Message} in topic: {Topic} - partition: {Partition} - offset: {Offset} ***",
                        result.Timestamp.UtcDateTime,
                        result.Value,
                        topic,
                        result.Partition.Value,
                        result.Offset.Value
                     );
                }
            }
            catch (ProduceException<Null, string> ex)
            {
                _logger.LogError($"===> Error occured: {ex.Message}");
                throw;
            }
        }
    }
}
