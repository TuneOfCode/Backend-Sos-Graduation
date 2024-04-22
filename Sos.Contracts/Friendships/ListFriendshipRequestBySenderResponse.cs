namespace Sos.Contracts.Friendships
{
    /// <summary>
    /// Represents the response of the friendship request by sender.
    /// </summary>
    public record FriendshipRequestBySenderResponse(
        Guid FriendshipRequestId,
        Guid ReceiverId,
        string ReceiverFullName,
        string ReceiverAvatar,
        string Status,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );

    /// <summary>
    /// Represents the response of the list friendship request by sender.
    /// </summary>
    public record ListFriendshipRequestBySenderResponse(
        IReadOnlyCollection<FriendshipRequestBySenderResponse> ListFriendshipRequestBySender
    );
}
