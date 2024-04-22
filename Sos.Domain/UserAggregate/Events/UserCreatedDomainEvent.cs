using Sos.Domain.Core.Events;

namespace Sos.Domain.UserAggregate.Events
{
    /// <summary>
    /// Represents the event that is raised when a user is created.
    /// </summary>
    public record UserCreatedDomainEvent(User User) : IDomainEvent;
}
