using Sos.Domain.Core.Events;

namespace Sos.Domain.FriendshipAggregate.Events
{
    /// <summary>
    /// Represents the event that is fired when a friendship request is accepted
    /// </summary>
    public record class FriendshipRequestAcceptedDomainEvent(FriendshipRequest FriendshipRequest) : IDomainEvent;
}
