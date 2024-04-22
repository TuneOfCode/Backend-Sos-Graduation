namespace Sos.Contracts.Friendships
{
    /// <summary>
    /// Represents the request of a friendship request.
    /// </summary>
    public record FriendshipRequestRequest(
        Guid SenderId,
        Guid ReceiverId
    );
}
