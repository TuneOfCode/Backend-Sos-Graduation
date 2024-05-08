using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sos.Application.Core.Abstractions.MessageQueue;
using Sos.Contracts.Socket;
using Sos.Infrastructure.MessageQueue.Settings;

namespace Sos.Infrastructure.MessageQueue
{
    public sealed class BackgroundKafkaService : BackgroundService
    {
        private readonly ILogger<KafkaProducer> _logger;
        private readonly IConsumer _consumer;

        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaConsumer"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="consumer">The consumer.</param>
        public BackgroundKafkaService(
            ILogger<KafkaProducer> logger,
            IConsumer consumer
        )
        {
            _logger = logger;
            _consumer = consumer;
        }

        // < inheritdoc />
        public async Task StartConsumerLoop(CancellationToken cancellationToken)
        {
            _logger.LogInformation("***** Kafka Consumer started *****");

            await _consumer.SubscribeAsync<LocationResponse>(
                MessageQueueConfiguration.SOS_TOPIC,
                MessageQueueConfiguration.SOS_FRIENSHIP_GROUP,
                cancellationToken
            );

            _logger.LogInformation("***** Kafka Consumer completed *****");

            await Task.CompletedTask;
        }

        // < inheritdoc />
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => StartConsumerLoop(stoppingToken), stoppingToken);
        }

        // < inheritdoc />
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
