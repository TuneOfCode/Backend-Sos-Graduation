using Sos.Application.Core.Abstractions.Messaging;
using Sos.Domain.Core.Commons.Result;

namespace Sos.Application.Modules.Friendships.Commands.AcceptFriendshipRequest
{
    /// <summary>
    /// Represents the accept friendship request command.
    /// </summary>
    public record AcceptFriendshipRequestCommand(
        Guid FriendshipRequestId
    ) : ICommand<Result>;
}
