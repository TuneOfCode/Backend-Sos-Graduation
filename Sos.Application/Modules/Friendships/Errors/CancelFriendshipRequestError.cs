using Sos.Domain.Core.Commons.Bases;

namespace Sos.Application.Modules.Friendships.Errors
{
    /// <summary>
    /// Contains the cancel friendship request errors.
    /// </summary>
    public static class CancelFriendshipRequestError
    {
        public static Error FriendshipRequestIsRequired =>
           new
           (
               "CancelFriendshipRequestError.FriendshipRequestIsRequired",
               "The friendship request identifier is required."
           );
    }
}
