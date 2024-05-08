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
        double Longitude,
        double Latitude,
        Guid FriendId,
        string FriendFullName,
        string FriendEmail,
        string FriendAvatar,
        double FriendLongitude,
        double FriendLatitude,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );
}
