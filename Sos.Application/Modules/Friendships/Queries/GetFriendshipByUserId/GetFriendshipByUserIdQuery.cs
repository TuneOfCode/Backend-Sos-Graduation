using Sos.Application.Core.Abstractions.Messaging;
using Sos.Contracts.Common;
using Sos.Contracts.Friendships;
using Sos.Domain.Core.Commons.Maybe;

namespace Sos.Application.Modules.Friendships.Queries.GetFriendshipByUserId
{
    /// <summary>
    /// Represents the get friendship by user id.
    /// </summary>
    public record GetFriendshipByUserIdQuery(
        Guid UserId,
        int Page,
        int PageSize
    ) : IQuery<Maybe<PaginateList<FriendshipResponse>>>;
}
