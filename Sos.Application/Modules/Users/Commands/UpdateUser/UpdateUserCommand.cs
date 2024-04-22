using Microsoft.AspNetCore.Http;
using Sos.Application.Core.Abstractions.Messaging;
using Sos.Contracts.Users;
using Sos.Domain.Core.Commons.Result;

namespace Sos.Application.Modules.Users.Commands.UpdateUser
{
    /// <summary>
    /// Represents the update user command.
    /// </summary>
    public record UpdateUserCommand(
        Guid UserId,
        string FirstName,
        string LastName,
        string ContactPhone,
        IFormFile Avatar
    ) : ICommand<Result<UserResponse>>;
}
