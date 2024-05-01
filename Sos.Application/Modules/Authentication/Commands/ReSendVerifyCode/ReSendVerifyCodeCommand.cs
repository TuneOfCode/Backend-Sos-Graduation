using Sos.Application.Core.Abstractions.Messaging;
using Sos.Contracts.Authentication;
using Sos.Domain.Core.Commons.Result;

namespace Sos.Application.Modules.Authentication.Commands.ReSendVerifyCode
{
    /// <summary>
    /// Represents the re-send verify code command.
    /// </summary>
    public record ReSendVerifyCodeCommand(
        string Email
    ) : ICommand<Result<ReSendVerifyCodeResponse>>;
}
