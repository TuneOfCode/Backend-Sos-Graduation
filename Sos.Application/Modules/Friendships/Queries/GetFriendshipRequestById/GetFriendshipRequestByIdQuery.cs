using Sos.Application.Core.Abstractions.Messaging;
using Sos.Contracts.Friendships;
using Sos.Domain.Core.Commons.Maybe;

namespace Sos.Application.Modules.Friendships.Queries.GetFriendshipRequestById
{
    /// <summary>
    /// Represents a query for getting a friendship request by identifier.
    /// </summary>
    public record GetFriendshipRequestByIdQuery(Guid FriendshipRequestId)
        : IQuery<Maybe<FriendshipRequestResponse>>;
}