using FluentValidation;
using Sos.Application.Core.Extensions;
using Sos.Application.Modules.Authentication.Errors;

namespace Sos.Application.Modules.Authentication.Commands.Verify
{
    /// <summary>
    /// Represents the verify command validator.
    /// </summary>
    public sealed class VerifyCommandValidator : AbstractValidator<VerifyCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VerifyCommandValidator"/> class.
        /// </summary>
        public VerifyCommandValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithError(VerifyError.CodeIsRequired);
        }
    }
}
