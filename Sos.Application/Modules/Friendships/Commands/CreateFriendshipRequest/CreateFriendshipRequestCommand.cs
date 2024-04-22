using Sos.Application.Core.Abstractions.Messaging;
using Sos.Contracts.Friendships;
using Sos.Domain.Core.Commons.Result;

namespace Sos.Application.Modules.Friendships.Commands.CreateFriendshipRequest
{
    public record CreateFriendshipRequestCommand(
        Guid SenderId,
        Guid ReceiverId
    ) : ICommand<Result<FriendshipRequestResponse>>;
}
