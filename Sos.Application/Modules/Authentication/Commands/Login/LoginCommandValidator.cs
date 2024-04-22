using FluentValidation;
using Sos.Application.Core.Extensions;
using Sos.Application.Modules.Authentication.Errors;

namespace Sos.Application.Modules.Authentication.Commands.Login
{
    public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginCommandValidator"/> class.
        /// </summary>
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithError(LoginError.EmailIsRequired);

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithError(LoginError.PasswordIsRequired);
        }
    }
}
