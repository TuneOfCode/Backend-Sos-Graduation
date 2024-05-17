using Sos.Application.Core.Abstractions.Messaging;
using Sos.Contracts.Notifications;
using Sos.Domain.Core.Commons.Maybe;

namespace Sos.Application.Modules.Notifications.Queries.GetNotificationsByUserId
{
    /// <summary>
    /// Represents the get notifications by user id query.
    /// </summary>
    public record GetNotificationsByUserIdQuery(Guid UserId) : IQuery<Maybe<List<NotificationResponse>>>;
}
