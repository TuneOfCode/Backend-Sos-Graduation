using Sos.Domain.Core.Events;

namespace Sos.Domain.FriendshipAggregate.Events
{
    /// <summary>
    /// Represents the event that is fired when a friendship request is created
    /// </summary>
    public record FriendshipRequestCreatedDomainEvent(FriendshipRequest FriendshipRequest) : IDomainEvent;
}
