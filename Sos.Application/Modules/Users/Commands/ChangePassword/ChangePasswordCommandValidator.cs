using FluentValidation;
using Sos.Application.Core.Extensions;
using Sos.Application.Modules.Users.Errors;

namespace Sos.Application.Modules.Users.Commands.ChangePassword
{
    /// <summary>
    /// Represents the <see cref="ChangePasswordCommand"/> validator.
    /// </summary>
    public sealed class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangePasswordCommandValidator"/> class.
        /// </summary>
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithError(ChangePasswordError.UserIdIsRequired);

            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .WithError(ChangePasswordError.CurrentPasswordIsRequired);

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithError(ChangePasswordError.PasswordIsRequired);

            RuleFor(x => x.Password)
                .NotEqual(x => x.CurrentPassword)
                .WithError(ChangePasswordError.PasswordCannotBeTheSameAsCurrentPassword);


            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .WithError(ChangePasswordError.ConfirmPasswordIsRequired);

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithError(ChangePasswordError.ConfirmPasswordDoesNotMatch);
        }
    }
}
