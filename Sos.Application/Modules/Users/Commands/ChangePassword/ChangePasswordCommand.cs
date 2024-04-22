using Sos.Application.Core.Abstractions.Messaging;
using Sos.Domain.Core.Commons.Result;

namespace Sos.Application.Modules.Users.Commands.ChangePassword
{
    /// <summary>
    /// Represents the change password command.
    /// </summary>
    public record ChangePasswordCommand(
        Guid UserId,
        string CurrentPassword,
        string Password,
        string ConfirmPassword) : ICommand<Result>;
}
