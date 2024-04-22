using Sos.Domain.Core.Commons.Bases;

namespace Sos.Application.Modules.Friendships.Errors
{
    /// <summary>
    /// Contains the create friendship request errors.
    /// </summary>
    public static class CreateFriendshipRequestError
    {
        public static Error SenderIdIsRequired =>
            new
            (
                "CreateFriendshipRequestError.SenderIdIsRequired",
                "The sender identitifier is required."
            );

        public static Error ReceiverIdIsRequired =>
            new
            (
                "CreateFriendshipRequestError.ReceiverIdIsRequired",
                "The receiver identitifier is required."
            );
    }
}
