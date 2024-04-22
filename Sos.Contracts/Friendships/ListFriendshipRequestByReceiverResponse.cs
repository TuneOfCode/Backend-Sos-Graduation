namespace Sos.Contracts.Friendships
{
    /// <summary>
    /// Represents the response of the friendship request by receiver.
    /// </summary>
    public record FriendshipRequestByReceiverResponse(
        Guid FriendshipRequestId,
        Guid SenderId,
        string SenderFullName,
        string SenderAvatar,
        string Status,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );

    /// <summary>
    /// Represents the response of the list friendship request by receiver.
    /// </summary>
    public record ListFriendshipRequestByReceiverResponse(
        IReadOnlyCollection<FriendshipRequestByReceiverResponse> ListFriendshipRequestByReceiver
    );
}
