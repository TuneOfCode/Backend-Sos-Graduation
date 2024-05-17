using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sos.Api.Configurations;
using Sos.Application.Modules.Notifications.Queries.GetNotificationsByUserId;
using Sos.Contracts.Notifications;
using Sos.Domain.Core.Commons.Maybe;

namespace Sos.Api.Modules.Notifications
{
    [Authorize]
    public sealed class NotificationsController : ApiController
    {
        private readonly IMediator _mediator;

        public NotificationsController(IMediator mediator) : base(mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(NotificationsRoute.GetNotificationsByUserId)]
        [ProducesResponseType(typeof(List<NotificationResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetNotificationsByUserId(Guid userId) =>
            await Maybe<GetNotificationsByUserIdQuery>
                .From(new GetNotificationsByUserIdQuery(userId))
                .Bind(query => _mediator.Send(query))
                .Match(Ok, NotFound);
    }
}
