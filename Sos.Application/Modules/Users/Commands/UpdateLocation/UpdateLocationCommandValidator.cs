using FluentValidation;
using Sos.Application.Core.Extensions;
using Sos.Application.Modules.Users.Errors;

namespace Sos.Application.Modules.Users.Commands.UpdateLocation
{
    /// <summary>
    /// Represents the validator for the command to update user location.
    /// </summary>
    public sealed class UpdateLocationCommandValidator : AbstractValidator<UpdateLocationCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateLocationCommandValidator"/> class.
        /// </summary>
        public UpdateLocationCommandValidator()
        {
            RuleFor(x => x.Longitude)
                .NotEmpty()
                 .WithError(UpdateLocationError.LongitudeIsRequired);

            RuleFor(x => x.Latitude)
                .NotEmpty()
                 .WithError(UpdateLocationError.LatitudeIsRequired);
        }
    }
}
