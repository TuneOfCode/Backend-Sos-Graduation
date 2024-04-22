namespace Sos.Domain.FriendshipAggregate.Enums
{
    /// <summary>
    /// Represents the status of a friend request.
    /// </summary>
    public static class StatusEnum
    {
        public const string Pending = nameof(Pending);

        public const string Accepted = nameof(Accepted);

        public const string Rejected = nameof(Rejected);
    }
}
