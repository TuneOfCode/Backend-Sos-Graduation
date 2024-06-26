﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Sos.Api.Configurations;
using Sos.Application.Core.Abstractions.Cache;
using Sos.Application.Core.Abstractions.Socket;
using Sos.Application.Modules.Friendships.Commands.AcceptFriendshipRequest;
using Sos.Application.Modules.Friendships.Commands.CancelFriendshipRequest;
using Sos.Application.Modules.Friendships.Commands.CreateFriendshipRequest;
using Sos.Application.Modules.Friendships.Commands.RejectFriendshipRequest;
using Sos.Application.Modules.Friendships.Commands.RemoveFriendship;
using Sos.Application.Modules.Friendships.Queries.GetFriendshipByUserId;
using Sos.Application.Modules.Friendships.Queries.GetFriendshipRecommendByUserId;
using Sos.Application.Modules.Friendships.Queries.GetFriendshipRequestById;
using Sos.Application.Modules.Friendships.Queries.GetFriendshipRequestByReceiver;
using Sos.Application.Modules.Friendships.Queries.GetFriendshipRequestBySender;
using Sos.Contracts.Common;
using Sos.Contracts.Friendships;
using Sos.Contracts.Notifications;
using Sos.Contracts.Users;
using Sos.Domain.Core.Commons.Maybe;
using Sos.Domain.Core.Commons.Result;
using Sos.Domain.Core.Errors;
using Sos.Domain.FriendshipAggregate.Repositories;
using Sos.Domain.UserAggregate.Repositories;
using Sos.Infrastructure.Socket;

namespace Sos.Api.Modules.Friendships
{
    public sealed class FriendshipsController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;
        private readonly IFriendshipRequestRepository _friendshipRequestRepository;
        private readonly IHubContext<NotificationsHub, INotificationsClient> _notificationsHubContext;
        private readonly ICacheService _cacheService;

        public FriendshipsController(
            IMediator mediator,
            IUserRepository userRepository,
            IFriendshipRequestRepository friendshipRequestRepository,
            IHubContext<NotificationsHub, INotificationsClient> notificationsHubContext,
            ICacheService cacheService

        ) : base(mediator)
        {
            _mediator = mediator;
            _userRepository = userRepository;
            _friendshipRequestRepository = friendshipRequestRepository;
            _notificationsHubContext = notificationsHubContext;
            _cacheService = cacheService;
        }

        [HttpGet(FriendshipsRoute.GetFriendshipRequestById)]
        [ProducesResponseType(typeof(FriendshipRequestResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFriendshipRequestById(Guid friendshipRequestId) =>
            await Maybe<GetFriendshipRequestByIdQuery>
                .From(new GetFriendshipRequestByIdQuery(friendshipRequestId))
                .Bind(query => _mediator.Send(query))
                .Match(Ok, NotFound);

        [HttpGet(FriendshipsRoute.GetFriendshipRequestBySenderId)]
        [ProducesResponseType(typeof(ListFriendshipRequestBySenderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFriendshipRequestBySenderId(
            Guid senderId) =>
            await Maybe<GetFriendshipRequestBySenderQuery>
                .From(new GetFriendshipRequestBySenderQuery(senderId))
                .Bind(query => _mediator.Send(query))
                .Match(Ok, NotFound);

        [HttpGet(FriendshipsRoute.GetFriendshipRequestByReceiverId)]
        [ProducesResponseType(typeof(ListFriendshipRequestByReceiverResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFriendshipRequestByReceiverId(
            Guid receiverId) =>
            await Maybe<GetFriendshipRequestByReceiverQuery>
                .From(new GetFriendshipRequestByReceiverQuery(receiverId))
                .Bind(query => _mediator.Send(query))
                .Match(Ok, NotFound);

        [HttpPost(FriendshipsRoute.CreateFriendshipRequest)]
        [ProducesResponseType(typeof(FriendshipRequestResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateFriendshipRequest(
            [FromBody] FriendshipRequestRequest friendshipRequestRequest)
        {
            var sender = (await _userRepository.GetByIdAsync(friendshipRequestRequest.SenderId)).Value;

            var newNotificationItem = new NotificationRequest(
                    Guid.NewGuid(),
                    "Lời mời kết bạn",
                    $"{sender.FullName} đã gửi lời mời kết bạn đến bạn",
                    sender.Avatar!.AvatarUrl,
                    DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            );

            var jsonContent = JsonConvert.SerializeObject(newNotificationItem);

            var result = await Result.Create(friendshipRequestRequest, GeneralError.UnProcessableRequest)
                .Map(request => new CreateFriendshipRequestCommand(
                    request.SenderId,
                    request.ReceiverId
                 ))
                .Bind(command => _mediator.Send(command))
                .Match(Ok, BadRequest);

            await _cacheService.RemoveAsync(Request.Path);

            var receiverId = friendshipRequestRequest.ReceiverId.ToString();

            await SaveNotificationAsync(_cacheService, receiverId, newNotificationItem);

            await _notificationsHubContext
                .Clients
                .User(receiverId)
                .ReceiveNotification(jsonContent);

            return result;
        }

        [HttpPatch(FriendshipsRoute.AcceptFriendshipRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AcceptFriendshipRequest(
            Guid friendshipRequestId)
        {
            var friendshipRequest = await _friendshipRequestRepository.GetByIdAsync(friendshipRequestId);

            var receiver = (await _userRepository.GetByIdAsync(friendshipRequest.Value.ReceiverId)).Value;

            var newNotificationItem = new NotificationRequest(
                    Guid.NewGuid(),
                    "Lời mời kết bạn",
                    $"{receiver.FullName} đã chấp nhận lời mời kết bạn của bạn",
                    receiver.Avatar!.AvatarUrl,
                    DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            );

            var jsonContent = JsonConvert.SerializeObject(newNotificationItem);

            var senderId = friendshipRequest.Value.SenderId.ToString();

            await SaveNotificationAsync(_cacheService, senderId, newNotificationItem);

            await _notificationsHubContext
                .Clients
                .User(senderId)
                .ReceiveNotification(jsonContent);

            var result = await Result.Success(new AcceptFriendshipRequestCommand(friendshipRequestId))
                .Bind(command => _mediator.Send(command))
                .Match(Ok, BadRequest);

            return result;
        }

        [HttpPatch(FriendshipsRoute.RejectFriendshipRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RejectFriendshipRequest(
            Guid friendshipRequestId) =>
            await Result.Success(new RejectFriendshipRequestCommand(friendshipRequestId))
                .Bind(command => _mediator.Send(command))
                .Match(Ok, BadRequest);

        [HttpPatch(FriendshipsRoute.CancelFriendshipRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelFriendshipRequest(
            Guid friendshipRequestId) =>
            await Result.Success(new CancelFriendshipRequestCommand(friendshipRequestId))
                .Bind(command => _mediator.Send(command))
                .Match(Ok, BadRequest);

        [HttpGet(FriendshipsRoute.GetFriendshipByUserId)]
        [ProducesResponseType(typeof(PaginateList<FriendshipResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFriendshipByUserId(
            Guid userId, [FromQuery] PaginateRequest paginateRequest) =>
            await Maybe<GetFriendshipByUserIdQuery>
                .From(new GetFriendshipByUserIdQuery(userId, paginateRequest.Page, paginateRequest.PageSize))
                .Bind(query => _mediator.Send(query))
                .Match(Ok, NotFound);

        // [Cache]
        [HttpGet(FriendshipsRoute.GetFriendshipRecommendByUserId)]
        [ProducesResponseType(typeof(PaginateList<UserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFriendshipRecommendByUserId(
            Guid userId, [FromQuery] PaginateRequest paginateRequest) =>
            await Maybe<GetFriendshipRecommendByUserIdQuery>
                .From(new GetFriendshipRecommendByUserIdQuery(userId, paginateRequest.Page, paginateRequest.PageSize))
                .Bind(query => _mediator.Send(query))
                .Match(Ok, NotFound);

        [HttpPatch(FriendshipsRoute.RemoveFriendshipByUserId)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveFriendshipByUserId(
            Guid userId, Guid friendId) =>
            await Result.Success(new RemoveFriendshipCommand(userId, friendId))
                .Bind(command => _mediator.Send(command))
                .Match(Ok, BadRequest);

        private static async Task SaveNotificationAsync(ICacheService cacheService, string userId, NotificationRequest newNotificationItem)
        {
            var notificationKey = $"notification_{userId}";

            var notificationsCached = await cacheService
                .GetAsync(notificationKey);

            if (notificationsCached == null)
            {
                var newNotifications = new List<NotificationRequest>
                {
                    newNotificationItem,
                };

                await cacheService.SetAsync(notificationKey, newNotifications, TimeSpan.FromMinutes(3));
            }
            else
            {
                var notifications = JsonConvert
                    .DeserializeObject<List<NotificationRequest>>(notificationsCached);

                if (notifications != null)
                {
                    notifications.Add(newNotificationItem);

                    await cacheService.SetAsync(notificationKey, notifications, TimeSpan.FromMinutes(3));
                }
            }
        }
    }
}
