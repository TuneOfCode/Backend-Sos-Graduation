using Microsoft.Extensions.Logging;
using Sos.Application.Core.Abstractions.Messaging;
using Sos.Domain.FriendshipAggregate.Events;

namespace Sos.Application.Modules.Friendships.EventHandlers
{
    public sealed class FriendshipRequestAcceptedDomainEventHandler
        : IEventHandler<FriendshipRequestAcceptedDomainEvent>
    {
        private readonly ILogger<FriendshipRequestAcceptedDomainEvent> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendshipRequestAcceptedDomainEventHandler"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public FriendshipRequestAcceptedDomainEventHandler(ILogger<FriendshipRequestAcceptedDomainEvent> logger)
        {
            _logger = logger;
        }

        public Task Handle(FriendshipRequestAcceptedDomainEvent notification, CancellationToken cancellationToken)
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
