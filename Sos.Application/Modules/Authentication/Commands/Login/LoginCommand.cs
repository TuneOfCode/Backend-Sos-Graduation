using Sos.Application.Core.Abstractions.Messaging;
using Sos.Contracts.Authentication;
using Sos.Domain.Core.Commons.Result;

namespace Sos.Application.Modules.Authentication.Commands.Login
{
    /// <summary>
    /// Represents the login command
    /// </summary>
    /// <param name="Email">The email of the user.</param>
    /// <param name="Password">The password not hashed.</param>
    public record LoginCommand(string Email, string Password) : ICommand<Result<TokenResponse>>;
}
