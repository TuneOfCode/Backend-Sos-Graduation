namespace Sos.Application.Core.Abstractions.Socket
{
    /// <summary>
    /// Represents the WebRTCs client interface.
    /// </summary>
    public interface IWebRTCsClient
    {
        /// <summary>
        /// Called when a user is connected.
        /// </summary>
        /// <param name="userId">The user indentifier.</param>
        /// <returns></returns>
        Task UserConnected(string userId);

        /// <summary>
        /// Called when a user is disconnected.
        /// </summary>
        /// <param name="userId">The user indentifier.</param>
        /// <returns></returns>
        Task UserDisconnected(string userId);

        /// <summary>
        /// Called when an ice candidate is received.
        /// </summary>
        /// <param name="candidate">The candidate.</param>
        /// <returns></returns>
        Task ReceiveIceCandidate(string candidate);

        /// <summary>
        /// Called when an incomming call is received.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task IncommingCall(string userId);

        /// <summary>
        /// Called when a call is accepted.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task CallAccepted(string userId);

        /// <summary>
        /// Called when a call is connected.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task CallConnected(string userId);

        /// <summary>
        /// Called when a call is denied.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task CallDenied(string userId);

        /// <summary>
        /// Called when a call is left.
        /// </summary>
        /// <param name="userId">The user indentifier.</param>
        /// <returns></returns>
        Task CallLeft(string userId);

        /// <summary>
        /// From user calls an offer when an comming call is accepted.
        /// </summary>
        /// <param name="offer">The offer with the user.</param>
        /// <returns></returns>
        Task Offer(string offer);

        /// <summary>
        /// From user calls an offer answer when an comming call is accepted.
        /// </summary>
        /// <param name="answer">The offer answer with the user.</param>
        /// <returns></returns>
        Task OfferAnswer(string answer);
    }
}
