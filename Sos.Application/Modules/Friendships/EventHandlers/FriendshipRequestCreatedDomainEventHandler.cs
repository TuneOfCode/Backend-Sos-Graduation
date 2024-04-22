using Microsoft.Extensions.Logging;
using Sos.Application.Core.Abstractions.Messaging;
using Sos.Domain.FriendshipAggregate.Events;

namespace Sos.Application.Modules.Friendships.EventHandlers
{
    public sealed class FriendshipRequestCreatedDomainEventHandler : IEventHandler<FriendshipRequestCreatedDomainEvent>
    {
        private readonly ILogger<FriendshipRequestCreatedDomainEvent> _logger;

        public FriendshipRequestCreatedDomainEventHandler(ILogger<FriendshipRequestCreatedDomainEvent> logger)
        {
            _logger = logger;
        }

        // <inheritdoc/>
        public Task Handle(FriendshipRequestCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("*** [{DateTime}] Sos Domain Event: {DomainEvent} ***",
                 DateTime.Now,
                 notification.GetType().Name
              );

            // TODO: Add event real-time processing

            return Task.CompletedTask;
        }
    }
}
