using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Sos.Application.Core.Abstractions.Socket;
using System.Collections.Concurrent;

namespace Sos.Infrastructure.Socket
{
    /// <summary>
    /// Represents a hub for sending notifications.
    /// </summary>
    [Authorize]
    public sealed class NotificationsHub : Hub<INotificationsClient>
    {
        private static readonly ConcurrentDictionary<string, string> userConnections = new();

        // <inheritdoc />
        public override Task OnConnectedAsync()
        {
            string userId = Context.UserIdentifier!;
            string connectionId = Context.ConnectionId;

            if (userConnections.ContainsKey(userId))
            {
                string oldConnectionId;
                userConnections.TryRemove(userId, out oldConnectionId!);
                Clients.Client(oldConnectionId).ForceDisconnect();
            }

            userConnections.TryAdd(userId, connectionId);

            return base.OnConnectedAsync();
        }

        /// <summary>
        /// Sends a notification from the client.
        /// </summary>
        /// <param name="content">The content of the notification.</param>
        /// <returns></returns>
        public async Task SendNotification(string content)
        {
            Console.WriteLine($"===> Context.UserIdentifier!: {Context.UserIdentifier!}");
            await Clients.User(Context.UserIdentifier!).ReceiveNotification(content);
        }

        /// <summary>
        /// Logs out the user.
        /// </summary>
        /// <returns></returns>
        public async Task Logout()
        {
            string userId = Context.UserIdentifier!;
            string connectionId;
            userConnections.TryRemove(userId, out connectionId!);
            await Clients.Client(connectionId).ForceDisconnect();
            await Clients.User(Context.UserIdentifier!).ForceDisconnect();
        }

        // <inheritdoc />
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            string userId = Context.UserIdentifier!;
            string connectionId;
            userConnections.TryRemove(userId, out connectionId!);
            Clients.Client(connectionId).ForceDisconnect();
            return base.OnDisconnectedAsync(exception);
        }
    }
}
