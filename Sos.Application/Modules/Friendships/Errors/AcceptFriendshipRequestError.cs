using Sos.Domain.Core.Commons.Bases;

namespace Sos.Application.Modules.Friendships.Errors
{
    /// <summary>
    /// Contains the accept friendship request errors.
    /// </summary>
    public static class AcceptFriendshipRequestError
    {
        public static Error FriendshipRequestIsRequired =>
            new
            (
                "AcceptFriendshipRequestError.FriendshipRequestIsRequired",
                "The friendship request identifier is required."
            );
    }
}
