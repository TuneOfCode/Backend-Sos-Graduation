using Sos.Application.Core.Abstractions.Messaging;
using Sos.Contracts.Friendships;
using Sos.Domain.Core.Commons.Maybe;

namespace Sos.Application.Modules.Friendships.Queries.GetFriendshipRequestByReceiver
{
    /// <summary>
    /// Represents the query for getting the list of friendship request by receiver.
    /// </summary>
    public record GetFriendshipRequestByReceiverQuery(
        Guid ReceiverId
    ) : IQuery<Maybe<ListFriendshipRequestByReceiverResponse>>;
}
