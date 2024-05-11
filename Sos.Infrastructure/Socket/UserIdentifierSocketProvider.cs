using Microsoft.AspNetCore.SignalR;
using Sos.Infrastructure.Authentication;
using System.Security.Claims;

namespace Sos.Infrastructure.Socket
{
    /// <summary>
    /// Represents a provider for user identifiers for the socket.
    /// </summary>
    internal sealed class UserIdentifierSocketProvider : IUserIdProvider
    {
        // <inheritdoc />
        public string? GetUserId(HubConnectionContext connection)
        {
            // Console.WriteLine($"===> GetUserId: {connection.User.FindFirstValue(ClaimsType.UserId)}");
            return connection.User.FindFirstValue(ClaimsType.UserId);
        }
    }
}
