using Sos.Application.Core.Abstractions.Messaging;
using Sos.Domain.Core.Commons.Result;

namespace Sos.Application.Modules.Friendships.Commands.RejectFriendshipRequest
{
    /// <summary>
    /// Represents the reject friendship request command.
    /// </summary>
    public record RejectFriendshipRequestCommand(
        Guid FriendshipRequestId
    ) : ICommand<Result>;
}
