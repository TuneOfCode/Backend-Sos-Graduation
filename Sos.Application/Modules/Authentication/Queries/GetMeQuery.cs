using Sos.Application.Core.Abstractions.Messaging;
using Sos.Contracts.Users;
using Sos.Domain.Core.Commons.Maybe;

namespace Sos.Application.Modules.Authentication.Queries
{
    /// <summary>
    /// Represents a query for getting current user
    /// </summary>
    public record GetMeQuery() : IQuery<Maybe<UserResponse>>;
}
