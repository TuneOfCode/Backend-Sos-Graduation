using Sos.Application.Core.Abstractions.Messaging;
using Sos.Domain.Core.Commons.Result;

namespace Sos.Application.Modules.Users.Commands.UpdateLocation
{
    /// <summary>
    /// Represents the command to update user location.
    /// </summary>
    public record UpdateLocationCommand(
        Guid UserId,
        double Longitude,
        double Latitude
    ) : ICommand<Result>;
}
