using FluentValidation;
using Sos.Application.Core.Extensions;
using Sos.Application.Modules.Authentication.Errors;

namespace Sos.Application.Modules.Authentication.Commands.RefreshToken
{
    /// <summary>
    /// Represents the refresh token validator.
    /// </summary>
    public sealed class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshTokenCommandValidator"/> class.
        /// </summary>
        public RefreshTokenCommandValidator()
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
