using Sos.Domain.Core.Events;

namespace Sos.Domain.UserAggregate.Events
{
    /// <summary>
    /// Represents the event that is raised when a users password is changed.
    /// </summary>
    public record UserPasswordChangedDomainEvent(User User) : IDomainEvent;
}
