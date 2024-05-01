namespace Sos.Application.Core.Abstractions.Socket
{
    /// <summary>
    /// Represents a notifications client interface.
    /// </summary>
    public interface INotificationsClient
    {
        /// <summary>
        /// Forces the client to disconnect.
        /// </summary>
        /// <returns></returns>
        Task ForceDisconnect();

        /// <summary>
        /// Receives a notification from the client.
        /// </summary>
        /// <param name="content">The content value.</param>
        /// <returns></returns>
        Task ReceiveNotification(string content);
    }
}
