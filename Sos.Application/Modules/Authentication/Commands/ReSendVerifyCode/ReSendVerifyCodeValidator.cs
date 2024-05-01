using FluentValidation;
using Sos.Application.Core.Extensions;
using Sos.Application.Modules.Authentication.Errors;

namespace Sos.Application.Modules.Authentication.Commands.ReSendVerifyCode
{
    /// <summary>
    /// Represents the re-send verify code validator.
    /// </summary>
    public sealed class ReSendVerifyCodeValidator : AbstractValidator<ReSendVerifyCodeCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReSendVerifyCodeValidator"/> class.
        /// </summary>
        public ReSendVerifyCodeValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithError(ReSendVerifyCodeError.EmailIsRequired);
        }
    }
}
