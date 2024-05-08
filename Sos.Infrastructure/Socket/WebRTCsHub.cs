using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Sos.Application.Core.Abstractions.Socket;
using Sos.Contracts.Socket;

namespace Sos.Infrastructure.Socket
{
    /// <summary>
    /// Represents a hub for WebRTC.
    /// </summary>
    [Authorize]
    public class WebRTCsHub : Hub<IWebRTCsClient>
    {
        private readonly IDictionary<string, SocketConnectionInfo> _usersConnected = new Dictionary<string, SocketConnectionInfo>();

        /// <summary>
        /// Connects the user.
        /// </summary>
        /// <returns></returns>
        public async Task<SocketConnectionInfo> Connect()
        {
            string userId = Context.UserIdentifier!;
            if (!_usersConnected.ContainsKey(userId))
            {
                _usersConnected.Add(userId,
                    new SocketConnectionInfo
                    (
                        Context.ConnectionId,
                        userId
                    )
                );

                await Clients.All.UserConnected(userId);
                Console.WriteLine($"===> {userId} is online");
            }

            return _usersConnected[userId];
        }

        /// <summary>
        /// Disconnects the user.
        /// </summary>
        /// <returns></returns>
        public async Task Disconnect()
        {
            string userId = Context.UserIdentifier!;

            await Clients.All.UserDisconnected(userId);

            Console.WriteLine($"===> {userId} is offline");

            if (_usersConnected.ContainsKey(userId))
            {
                _usersConnected.Remove(userId);
            }
        }

        /// <summary>
        /// Sends an ICE candidate.
        /// </summary>
        /// <param name="toUserId">To the user identifier.</param>
        /// <param name="candidate">The candidate.</param>
        /// <returns></returns>
        public async Task SendIceCandidate(string toUserId, string candidate)
        {
            Console.WriteLine($"===> ICE candidate of {toUserId} is {candidate}");
            string fromUserId = Context.UserIdentifier!;
            await Clients.User(toUserId).ReceiveIceCandidate(candidate);
            // if (_usersConnected.ContainsKey(toUserId))
            // {
            //     await Clients.Client(_usersConnected[toUserId].ConnectionId).ReceiveIceCandidate(fromUserId, candidate);
            // }
        }

        /// <summary>
        /// Starts a call.
        /// </summary>
        /// <param name="toUserId">To the user identifier.</param>
        /// <returns></returns>
        public async Task StartCall(string toUserId)
        {
            string fromUserId = Context.UserIdentifier!;
            // Console.WriteLine($"===> Starting call from {fromUserId} to {toUserId}");
            await Clients.User(toUserId).IncommingCall(fromUserId);
            Console.WriteLine($"===> Started call from {fromUserId} to {toUserId}");
        }

        /// <summary>
        /// Accepts a call.
        /// </summary>
        /// <param name="fromUserId">From the user identifier.</param>
        /// <returns></returns>
        public async Task AcceptCall(string fromUserId)
        {
            string toUserId = Context.UserIdentifier!;
            await Clients.User(fromUserId).CallAccepted(toUserId);
            Console.WriteLine($"===> {toUserId} Accepted call of {fromUserId}");
            // if (_usersConnected.ContainsKey(fromUserId))
            // {
            //     await Clients.Client(_usersConnected[fromUserId].ConnectionId).CallAccepted(toUserId);

            //     await Clients.Client(_usersConnected[toUserId].ConnectionId).CallConnected(fromUserId);
            // }
        }

        /// <summary>
        /// Denies a call.
        /// </summary>
        /// <param name="fromUserId">From the user identifier.</param>
        /// <returns></returns>
        public async Task DenyCall(string fromUserId)
        {
            string toUserId = Context.UserIdentifier!;
            await Clients.User(fromUserId).CallDenied(toUserId);
            Console.WriteLine($"===> {toUserId} Denied call of {fromUserId}");
            // if (_usersConnected.ContainsKey(fromUserId))
            // {
            //     await Clients.Client(_usersConnected[fromUserId].ConnectionId).CallDenied(toUserId);
            // }
        }

        /// <summary>
        /// Leaves a call.
        /// </summary>
        /// <param name="toUserId">To the user identifier.</param>
        /// <returns></returns>
        public async Task LeaveCall(string toUserId)
        {
            string fromUserId = Context.UserIdentifier!;
            await Clients.User(toUserId).CallLeft(fromUserId);
            Console.WriteLine($"===> {fromUserId} Left call");
            // if (_usersConnected.ContainsKey(toUserId))
            // {
            //     await Clients.Client(_usersConnected[toUserId].ConnectionId).CallLeft(fromUserId);
            // }
        }

        /// <summary>
        /// Sends an offer.
        /// </summary>
        /// <param name="toUserId">To the user identifier.</param>
        /// <param name="offer">The offer.</param>
        /// <returns></returns>
        public async Task Offer(string toUserId, string offer)
        {
            string fromUserId = Context.UserIdentifier!;
            Console.WriteLine($"===> Offer of {toUserId}");
            await Clients.User(toUserId).Offer(offer);
            // if (_usersConnected.ContainsKey(toUserId))
            // {
            //     await Clients.Client(_usersConnected[toUserId].ConnectionId).Offer(fromUserId, offer);
            // }
        }

        /// <summary>
        /// Sends an offer answer.
        /// </summary>
        /// <param name="toUserId">To the user identifier.</param>
        /// <param name="offer">The offer.</param>
        /// <returns></returns>
        public async Task OfferAnswer(string toUserId, string offer)
        {
            string fromUserId = Context.UserIdentifier!;
            Console.WriteLine($"===> Offer answer of {toUserId}");
            await Clients.User(toUserId).OfferAnswer(offer);
            // if (_usersConnected.ContainsKey(toUserId))
            // {
            //     await Clients.Client(_usersConnected[toUserId].ConnectionId).OfferAnswer(fromUserId, offer);
            // }
        }
    }
}
