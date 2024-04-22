using Sos.Application.Core.Abstractions.Messaging;
using Sos.Contracts.Friendships;
using Sos.Domain.Core.Commons.Maybe;

namespace Sos.Application.Modules.Friendships.Queries.GetFriendshipRequestBySender
{
    /// <summary>
    /// Represents a query for getting a list of friend requests by sender.
    /// </summary>
    public record GetFriendshipRequestBySenderQuery(
        Guid SenderId
    ) : IQuery<Maybe<ListFriendshipRequestBySenderResponse>>;
}