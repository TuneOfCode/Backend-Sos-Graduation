using Microsoft.Extensions.Logging;
using Sos.Application.Core.Abstractions.Messaging;
using Sos.Domain.FriendshipAggregate.Events;

namespace Sos.Application.Modules.Friendships.EventHandlers
{
    public sealed class FriendshipRequestRejectedDomainEventHandler
        : IEventHandler<FriendshipRequestRejectedDomainEvent>
    {
        private readonly ILogger<FriendshipRequestRejectedDomainEvent> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendshipRequestRejectedDomainEventHandler"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public FriendshipRequestRejectedDomainEventHandler(ILogger<FriendshipRequestRejectedDomainEvent> logger)
        {
            _logger = logger;
        }

        public Task Handle(FriendshipRequestRejectedDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("*** [{DateTime}] Sos Domain Event: {DomainEvent} ***",
                 DateTime.Now,
                 notification.GetType().Name
              );

            return Task.CompletedTask;
        }
    }
}
