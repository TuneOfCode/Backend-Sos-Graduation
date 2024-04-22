using Sos.Domain.Core.Commons.Bases;

namespace Sos.Domain.FriendshipAggregate.Errors
{
    /// <summary>
    /// Contains the friendship domain errors.
    /// </summary>
    public static class FriendshipDomainError
    {
        public static Error AlreadyAccepted =>
            new
            (
                "FrienshipDomain.AlreadyAccepted",
                "The friendship request is already accepted."
            );

        public static Error AlreadyRejected =>
            new
            (
                "FrienshipDomain.AlreadyRejected",
                "The friendship request is already rejected."
            );

        public static Error SenderNotFound =>
            new
            (
                "FrienshipDomain.SenderNotFound",
                "The sender was not found."
            );

        public static Error ReceiverNotFound =>
            new
            (
                "FrienshipDomain.ReceiverNotFound",
                "The receiver was not found."
            );

        public static Error UserNotFound =>
            new
            (
                "FrienshipDomain.UserNotFound",
                "The user was not found."
            );

        public static Error FriendNotFound =>
            new
            (
                "FrienshipDomain.FriendNotFound",
                "The friend was not found."
            );

        public static Error SenderIsNotCurrentUser =>
            new
            (
                "FrienshipDomain.SenderIsNotCurrentUser",
                "The sender is not the current user."
            );

        public static Error ReceiverIsCurrentUser =>
            new
            (
                "FrienshipDomain.ReceiverIsCurrentUser",
                "The receiver is the current user."
            );

        public static Error SenderIsReceiver =>
            new
            (
                "FrienshipDomain.SenderIsReceiver",
                "The sender is the receiver."
            );

        public static Error AlreadyFriend =>
            new
            (
                "FrienshipDomain.AlreadyFriend",
                "The sender is a friend of the receiver."
            );

        public static Error FriendshipRequestNotFound =>
            new
            (
                "FrienshipDomain.FriendshipRequestNotFound",
                "The friendship request was not found."
            );

        public static Error FriendshipRequestIsExisted =>
            new
            (
                "FrienshipDomain.FriendshipRequestIsExisted",
                "The friendship request is existed."
            );

        public static Error NotAllowedToAccept =>
            new
            (
                "FrienshipDomain.NotAllowedToAccept",
                "The friendship request is not allowed to accept because the sender is not the current user."
            );

        public static Error NotAllowedToRemove =>
            new
            (
                "FrienshipDomain.NotAllowedToRemove",
                "The friendship is not allowed to remove because the sender is not the current user."
            );
    }
}
