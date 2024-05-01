using Sos.Application.Core.Abstractions.Messaging;
using Sos.Contracts.Common;
using Sos.Contracts.Users;
using Sos.Domain.Core.Commons.Maybe;

namespace Sos.Application.Modules.Friendships.Queries.GetFriendshipRecommendByUserId
{
    /// <summary>
    /// Represents the get friendship recommend by user id.
    /// </summary>
    public record GetFriendshipRecommendByUserIdQuery(
        Guid UserId,
        int Page,
        int PageSize
    ) : IQuery<Maybe<PaginateList<UserResponse>>>;
}
