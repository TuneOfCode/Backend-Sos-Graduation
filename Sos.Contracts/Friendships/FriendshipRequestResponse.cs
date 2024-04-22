namespace Sos.Contracts.Friendships
{
    /// <summary>
    /// Represents the response of a friendship request.
    /// </summary>
    public record FriendshipRequestResponse(
        Guid FriendshipRequestId,
        Guid SenderId,
        string SenderFullName,
        string SenderAvatar,
        Guid ReceiverId,
        string ReceiverFullName,
        string ReceiverAvatar,
        string Status,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );
}
