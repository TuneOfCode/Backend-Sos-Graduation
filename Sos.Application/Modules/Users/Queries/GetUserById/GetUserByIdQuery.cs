using Sos.Application.Core.Abstractions.Messaging;
using Sos.Contracts.Users;
using Sos.Domain.Core.Commons.Maybe;

namespace Sos.Application.Modules.Users.Queries.GetUserById
{
    /// <summary>
    /// Represents the get user by id query.
    /// </summary>
    public sealed record GetUserByIdQuery(Guid UserId) : IQuery<Maybe<UserResponse>>;
}
