using FluentValidation;
using Sos.Application.Core.Extensions;
using Sos.Application.Modules.Authentication.Errors;

namespace Sos.Application.Modules.Authentication.Commands.ReSendVerifyCode
{
    /// <summary>
    /// Represents the re-send verify code validator.
    /// </summary>
    public sealed class ReSendVerifyCodeCommandValidator : AbstractValidator<ReSendVerifyCodeCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReSendVerifyCodeCommandValidator"/> class.
        /// </summary>
        public ReSendVerifyCodeCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithError(ReSendVerifyCodeError.EmailIsRequired);
        }
    }
}
