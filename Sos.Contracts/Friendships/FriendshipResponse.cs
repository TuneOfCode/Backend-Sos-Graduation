namespace Sos.Contracts.Friendships
{
    /// <summary>
    /// Represents the response of a friendship.
    /// </summary>
    public record FriendshipResponse(
        Guid UserId,
        string FullName,
        string Email,
        string Avatar,
        Guid FriendId,
        string FriendFullName,
        string FriendEmail,
        string FriendAvatar,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );
}
