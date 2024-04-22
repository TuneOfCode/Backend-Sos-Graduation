using Sos.Domain.Core.Commons.Bases;

namespace Sos.Application.Modules.Friendships.Errors
{
    /// <summary>
    /// Contains the reject friendship request errors.
    /// </summary>
    public static class RejectFriendshipRequestError
    {
        public static Error FriendshipRequestIsRequired =>
            new
            (
                "RejectFriendshipRequestError.FriendshipRequestIsRequired",
                "The friendship request identifier is required."
            );
    }
}
