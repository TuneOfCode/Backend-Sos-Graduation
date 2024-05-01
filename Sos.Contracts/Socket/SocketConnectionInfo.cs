namespace Sos.Contracts.Socket
{
    /// <summary>
    /// Represents the socket connection info.
    /// </summary>
    public record SocketConnectionInfo(
        string ConnectionId,
        string UserId
    );
}
