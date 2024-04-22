namespace Sos.Api.Modules.Friendships
{
    /// <summary>
    /// Contains all routes for <see cref="FriendshipsController"/>
    /// </summary>
    public static class FriendshipsRoute
    {
        public const string GetFriendshipRequestById = "friendship-requests/{friendshipRequestId:guid}";

        public const string GetFriendshipRequestBySenderId = "friendship-requests/{senderId:guid}/sent";

        public const string GetFriendshipRequestByReceiverId = "friendship-requests/{receiverId:guid}/received";

        public const string CreateFriendshipRequest = "friendship-requests";

        public const string AcceptFriendshipRequest = "friendship-requests/{friendshipRequestId:guid}/accept";

        public const string RejectFriendshipRequest = "friendship-requests/{friendshipRequestId:guid}/reject";

        public const string CancelFriendshipRequest = "friendship-requests/{friendshipRequestId:guid}/cancel";

        public const string GetFriendshipByUserId = "friendships/{userId:guid}";

        public const string RemoveFriendshipByUserId = "friendships/{userId:guid}/remove/{friendId:guid}";
    }
}
