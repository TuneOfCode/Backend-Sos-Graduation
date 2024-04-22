using Sos.Application.Core.Abstractions.Messaging;
using Sos.Domain.Core.Commons.Result;

namespace Sos.Application.Modules.Friendships.Commands.RemoveFriendship
{
    /// <summary>
    /// Represents the remove friendship command.
    /// </summary>
    public record RemoveFriendshipCommand(
        Guid UserId,
        Guid FriendId
    ) : ICommand<Result>;
}
