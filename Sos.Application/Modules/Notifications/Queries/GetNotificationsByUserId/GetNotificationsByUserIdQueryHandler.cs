using Newtonsoft.Json;
using Sos.Application.Core.Abstractions.Authentication;
using Sos.Application.Core.Abstractions.Cache;
using Sos.Application.Core.Abstractions.Messaging;
using Sos.Contracts.Notifications;
using Sos.Domain.Core.Commons.Maybe;

namespace Sos.Application.Modules.Notifications.Queries.GetNotificationsByUserId
{
    /// <summary>
    /// Represents the get notifications by user id query handler.
    /// </summary>
    public sealed class GetNotificationsByUserIdQueryHandler : IQueryHandler<GetNotificationsByUserIdQuery, Maybe<List<NotificationResponse>>>
    {
        private readonly IUserIdentifierProvider _userIdentifierProvider;
        private readonly ICacheService _cacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetNotificationsByUserIdQueryHandler"/> class.
        /// </summary>
        /// <param name="userIdentifierProvider">The user identifier provider.</param>
        /// <param name="cacheService">The cache service.</param>
        public GetNotificationsByUserIdQueryHandler(
            IUserIdentifierProvider userIdentifierProvider,
            ICacheService cacheService
        )
        {
            _userIdentifierProvider = userIdentifierProvider;
            _cacheService = cacheService;
        }

        // < inheritdoc />
        public async Task<Maybe<List<NotificationResponse>>> Handle(GetNotificationsByUserIdQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId != _userIdentifierProvider.UserId)
            {
               return Maybe<List<NotificationResponse>>.None;
            }

            var notificationKey = $"notification_{request.UserId}";

            var notificationsCached = await _cacheService
                .GetAsync(notificationKey);

            var notifications = new List<NotificationResponse>();

            if (notificationsCached == null)
            {
                return notifications;
            }

            notifications = JsonConvert.DeserializeObject<List<NotificationResponse>>(notificationsCached);

            return notifications!.OrderByDescending(n => n.CreatedAt).ToList();
        }
    }
}
