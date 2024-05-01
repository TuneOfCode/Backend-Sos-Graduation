using FluentValidation;
using Sos.Application.Core.Extensions;
using Sos.Application.Modules.Authentication.Errors;

namespace Sos.Application.Modules.Authentication.Commands.RefreshToken
{
    /// <summary>
    /// Represents the refresh token validator.
    /// </summary>
    public sealed class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshTokenValidator"/> class.
        /// </summary>
        public RefreshTokenValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithError(RefreshTokenError.UserIsRequired);

            RuleFor(x => x.RefreshToken)
                .NotEmpty()
                .WithError(RefreshTokenError.RefreshTokenIsRequired);
        }
    }
}
