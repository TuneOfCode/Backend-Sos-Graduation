using Sos.Application.Core.Abstractions.Messaging;
using Sos.Domain.Core.Commons.Result;

namespace Sos.Application.Modules.Friendships.Commands.CancelFriendshipRequest
{
    /// <summary>
    /// Represents the cancel friendship request command.
    /// </summary>
    public record CancelFriendshipRequestCommand(
        Guid FriendshipRequestId
    ) : ICommand<Result>;
}