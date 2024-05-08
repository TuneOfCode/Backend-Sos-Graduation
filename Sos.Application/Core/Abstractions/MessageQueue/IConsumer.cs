namespace Sos.Application.Core.Abstractions.MessageQueue
{
    /// <summary>
    /// Represents interface for message consumer.
    /// </summary>
    public interface IConsumer
    {
        /// <summary>
        /// Subscribes to a topic.
        /// </summary>
        /// <typeparam name="T">The message type.</typeparam>
        /// <param name="topic">The topic to subscribe.</param>
        /// <param name="groupId">The group id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<T> SubscribeAsync<T>(string topic, string groupId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Unsubscribes to a topic.
        /// </summary>
        /// <typeparam name="T">The message type.</typeparam>
        /// <param name="topic">The topic to subscribe.</param>
        /// <param name="groupId">The group id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task UnsubscribeAsync(string topic, string groupId, CancellationToken cancellationToken = default);
    }
}