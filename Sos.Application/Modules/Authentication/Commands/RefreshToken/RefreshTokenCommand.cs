using Sos.Application.Core.Abstractions.Messaging;
using Sos.Contracts.Authentication;
using Sos.Domain.Core.Commons.Result;

namespace Sos.Application.Modules.Authentication.Commands.RefreshToken
{
    /// <summary>
    /// Represents the refresh token command.
    /// </summary>
    public record RefreshTokenCommand(
        Guid UserId,
        string RefreshToken
    ) : ICommand<Result<TokenResponse>>;
}
