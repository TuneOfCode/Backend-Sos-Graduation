using FluentValidation;
using Sos.Application.Core.Extensions;
using Sos.Application.Modules.Users.Errors;
using Sos.Domain.UserAggregate.Enums;

namespace Sos.Application.Modules.Users.Commands.UpdateUser
{
    public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateUserCommandValidator"/> class.
        /// </summary>
        public UpdateUserCommandValidator()
        {
            //RuleFor(x => x.FirstName)
            //    .NotNull()
            //    .WithMessage(CreateUserError.FirstNameIsRequired);

            RuleFor(x => x.FirstName)
                .MinimumLength(UserRequirementEnum.MinFirstNameLength)
                .WithError(CreateUserError.FirstNameIsTooShort)
                .MaximumLength(UserRequirementEnum.MaxFirstNameLength)
                .WithError(CreateUserError.FirstNameIsTooLong);

            //RuleFor(x => x.LastName)
            //    .NotNull()
            //    .WithMessage(CreateUserError.LastNameIsRequired);

            RuleFor(x => x.LastName)
                .MinimumLength(UserRequirementEnum.MinLastNameLength)
                .WithError(CreateUserError.LastNameIsTooShort)
                .MaximumLength(UserRequirementEnum.MaxLastNameLength)
                .WithError(CreateUserError.LastNameIsTooLong);

            //RuleFor(x => x.ContactPhone)
            //    .NotNull()
            //    .WithMessage(CreateUserError.ContactPhoneIsRequired);
        }
    }
}
