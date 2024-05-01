using Sos.Application.Core.Abstractions.Messaging;
using Sos.Domain.Core.Commons.Result;

namespace Sos.Application.Modules.Authentication.Commands.Verify
{
    /// <summary>
    /// Represents the verify command.
    /// </summary>
    public record VerifyCommand(
        string Email,
        string Code
    ) : ICommand<Result>;
}
