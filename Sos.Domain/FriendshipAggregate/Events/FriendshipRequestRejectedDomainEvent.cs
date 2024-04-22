using Sos.Domain.Core.Events;

namespace Sos.Domain.FriendshipAggregate.Events
{
    /// <summary>
    /// Represents the event that is fired when a friendship request is rejected
    /// </summary>
    public record FriendshipRequestRejectedDomainEvent(FriendshipRequest FriendshipRequest) : IDomainEvent;
}
