namespace Sos.Application.Core.Abstractions.MessageQueue
{
    /// <summary>
    /// Represents interface for message producer.
    /// </summary>
    public interface IProducer
    {
        /// <summary>
        /// Publishes message to topic.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        Task PublishAsync(string topic, object message);
    }
}
