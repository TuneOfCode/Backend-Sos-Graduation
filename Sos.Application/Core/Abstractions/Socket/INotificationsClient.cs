using Sos.Contracts.Socket;

namespace Sos.Application.Core.Abstractions.Socket
{
    /// <summary>
    /// Represents a notifications client interface.
    /// </summary>
    public interface INotificationsClient
    {
        /// <summary>
        /// Connects the client.
        /// </summary>
        /// <param name="usersConnected">The list of users connected.</param>
        /// <returns></returns>
        Task ConnectAsync(IList<string> usersConnected);

        /// <summary>
        /// Disconnects the client.
        /// </summary>
        /// <param name="usersConnected">The list of users connected.</param>
        /// <returns></returns>
        Task DisconnectAsync(IList<string> usersConnected);

        /// <summary>
        /// Receives a notification from the client.
        /// </summary>
        /// <param name="content">The content value.</param>
        /// <returns></returns>
        Task ReceiveNotification(string content);

        /// <summary>
        /// Receives the location of victim from the client.
        /// </summary>
        /// <param name="data">The location data.</param>
        /// <returns></returns>
        Task ReceiveLocation(string data);

        /// <summary>
        /// Trackes the location of victim from the client.
        /// </summary>
        /// <param name="location">The location value.</param>
        /// <returns></returns>
        Task TrackLocation(string location);

        /// <summary>
        /// Receives the safe from victim from the client.
        /// </summary>
        /// <param name="victimId">The victim id.</param>
        /// <returns></returns>
        Task ReceiveSafeFromVictim(string victimId);
    }
}
