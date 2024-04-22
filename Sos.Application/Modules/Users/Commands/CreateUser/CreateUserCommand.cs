using Sos.Application.Core.Abstractions.Messaging;
using Sos.Contracts.Users;
using Sos.Domain.Core.Commons.Result;

namespace Sos.Application.Modules.Users.Commands.CreateUser
{
    /// <summary>
    /// Represents the create friendship request command.
    /// </summary>
    public record CreateUserCommand(
        string FirstName,
        string LastName,
        string Email,
        string ContactPhone,
        string Password,
        string ConfirmPassword
    ) : ICommand<Result<UserResponse>>;
}
