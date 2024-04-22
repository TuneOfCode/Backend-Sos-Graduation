using Microsoft.Extensions.Logging;
using Sos.Application.Core.Abstractions.Messaging;
using Sos.Domain.UserAggregate.Events;

namespace Sos.Application.Modules.Users.EventHandlers
{
    /// <summary>
    /// Represents the domain event handler for the user created event.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="UserCreatedDomainEventHandler"/> class.
    /// </remarks>
    /// <param name="logger">The Microsoft logger.</param>
    public class UserCreatedDomainEventHandler(ILogger<UserCreatedDomainEventHandler> logger) : IEventHandler<UserCreatedDomainEvent>
    {
        private readonly ILogger<UserCreatedDomainEventHandler> _logger = logger;

        /// <inheritdoc />
        public Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("*** [{DateTime}] Sos Domain Event: {DomainEvent} ***",
                DateTime.Now,
                notification.GetType().Name
             );

            return Task.CompletedTask;
        }
    }
}
