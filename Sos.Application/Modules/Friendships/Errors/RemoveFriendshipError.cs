using Sos.Domain.Core.Commons.Bases;

namespace Sos.Application.Modules.Friendships.Errors
{
    /// <summary>
    /// Contains the remove friendship by user id errors.
    /// </summary>
    public static class RemoveFriendshipError
    {
        public static Error UserIsRequired =>
            new
            (
                "RemoveFriendshipByUserIdError.UserIsRequired",
                "The user identifier is required."
            );

        public static Error FriendIsRequired =>
            new
            (
                "RemoveFriendshipByUserIdError.FriendIsRequired",
                "The friend identifier is required."
            );
    }
}
