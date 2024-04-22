using Sos.Domain.Core.Events;

namespace Sos.Domain.UserAggregate.Events
{
    /// <summary>
    /// Represents the event that is raised when a user is updated.
    /// </summary>
    /// <param name="User">The updated user.</param>
    public record UserUpdatedDomainEvent(User User) : IDomainEvent;
}
