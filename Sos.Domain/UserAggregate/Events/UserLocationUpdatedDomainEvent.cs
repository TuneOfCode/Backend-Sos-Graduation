using Sos.Domain.Core.Events;

namespace Sos.Domain.UserAggregate.Events
{
    /// <summary>
    /// Represents the event that is raised when a user location is updated.
    /// </summary>
    public record UserLocationUpdatedDomainEvent(User user) : IDomainEvent;
}
