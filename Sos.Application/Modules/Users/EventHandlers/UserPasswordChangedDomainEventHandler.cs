using Microsoft.Extensions.Logging;
using Sos.Application.Core.Abstractions.Messaging;
using Sos.Domain.UserAggregate.Events;

namespace Sos.Application.Modules.Users.EventHandlers
{
    /// <summary>
    /// Represents the domain event handler for the user password changed event.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="UserPasswordChangedDomainEventHandler"/> class.
    /// </remarks>
    /// <param name="logger">The Microsoft logger.</param>
    public class UserPasswordChangedDomainEventHandler(ILogger<UserPasswordChangedDomainEventHandler> logger) : IEventHandler<UserPasswordChangedDomainEvent>
    {
        private readonly ILogger<UserPasswordChangedDomainEventHandler> _logger = logger;

        // / <inheritdoc />
        public Task Handle(UserPasswordChangedDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("*** [{DateTime}] Sos Domain Event: {DomainEvent} ***",
                DateTime.Now,
                notification.GetType().Name
             );

            return Task.CompletedTask;
        }
    }
}
