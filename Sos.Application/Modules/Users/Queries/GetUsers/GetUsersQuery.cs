using Sos.Application.Core.Abstractions.Messaging;
using Sos.Contracts.Common;
using Sos.Contracts.Users;
using Sos.Domain.Core.Commons.Maybe;

namespace Sos.Application.Modules.Users.Queries.GetUsers
{
    /// <summary>
    /// Represents the query to get users.
    /// </summary>
    public record GetUsersQuery(
        int Page,
        int PageSize
    ) : IQuery<Maybe<PaginateList<UserResponse>>>;
}
