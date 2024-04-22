using FluentValidation;
using Sos.Application.Core.Extensions;
using Sos.Application.Modules.Users.Errors;
using Sos.Domain.UserAggregate.Enums;

namespace Sos.Application.Modules.Users.Commands.CreateUser
{
    /// <summary>
    /// Represents the <see cref="CreateUserCommand"/> validator.
    /// </summary>
    public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserCommandValidator"/> class.
        /// </summary>
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                 .WithError(CreateUserError.FirstNameIsRequired);

            RuleFor(x => x.FirstName)
                .MinimumLength(UserRequirementEnum.MinFirstNameLength)
                .WithError(CreateUserError.FirstNameIsTooShort)
                .MaximumLength(UserRequirementEnum.MaxFirstNameLength)
                .WithError(CreateUserError.FirstNameIsTooLong);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithError(CreateUserError.LastNameIsRequired);

            RuleFor(x => x.LastName)
                .MinimumLength(UserRequirementEnum.MinLastNameLength)
                .WithError(CreateUserError.LastNameIsTooShort)
                .MaximumLength(UserRequirementEnum.MaxLastNameLength)
                .WithError(CreateUserError.LastNameIsTooLong);

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithError(CreateUserError.EmailIsRequired);

            RuleFor(x => x.ContactPhone)
                .NotEmpty()
                .WithError(CreateUserError.ContactPhoneIsRequired);

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithError(CreateUserError.PasswordIsRequired);

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .WithError(CreateUserError.ConfirmPasswordIsRequired);

            RuleFor(x => x.Password)
                .Equal(x => x.ConfirmPassword)
                .WithError(CreateUserError.ConfirmPasswordDoNotMatch);
        }
    }
}
