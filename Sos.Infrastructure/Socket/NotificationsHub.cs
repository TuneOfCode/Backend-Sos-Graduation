using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Sos.Application.Core.Abstractions.Cache;
using Sos.Application.Core.Abstractions.MessageQueue;
using Sos.Application.Core.Abstractions.Socket;
using Sos.Contracts.Socket;
using Sos.Domain.FriendshipAggregate.Repositories;
using Sos.Domain.UserAggregate.Repositories;
using Sos.Infrastructure.MessageQueue.Settings;
using System.Text.Json;

namespace Sos.Infrastructure.Socket
{
    /// <summary>
    /// Represents a hub for sending notifications.
    /// </summary>
    [Authorize]
    public sealed class NotificationsHub : Hub<INotificationsClient>
    {
        private static readonly IList<string> _usersConnected = new List<string>();
        private readonly IProducer _producer;
        private readonly IConsumer _consumer;
        private readonly IUserRepository _userRepository;
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IHubContext<NotificationsHub, INotificationsClient> _notificationsHubContext;
        private readonly ICacheService _cacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationsHub"/> class.
        /// </summary>
        /// <param name="producer">The producer.</param>
        /// <param name="consumer">The consumer.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="friendshipRepository">The friendship repository.</param>
        /// <param name="notificationsHubContext">The notifications hub context.</param>
        /// <param name="cacheService">The cache service.</param>
        public NotificationsHub(
            IProducer producer,
            IConsumer consumer,
            IUserRepository userRepository,
            IFriendshipRepository friendshipRepository,
            IHubContext<NotificationsHub, INotificationsClient> notificationsHubContext,
            ICacheService cacheService
        )
        {
            _producer = producer;
            _consumer = consumer;
            _userRepository = userRepository;
            _friendshipRepository = friendshipRepository;
            _notificationsHubContext = notificationsHubContext;
            _cacheService = cacheService;
        }

        // <inheritdoc />
        public override Task OnConnectedAsync()
        {
            string userId = Context.UserIdentifier!;
            if (!_usersConnected.Contains(userId))
            {
                _usersConnected.Add(userId);
            }

            Clients.All.ConnectAsync(_usersConnected);

            return base.OnConnectedAsync();
        }

        /// <summary>
        /// Sends a notification from the client.
        /// </summary>
        /// <param name="content">The content of the notification.</param>
        /// <returns></returns>
        public async Task SendNotification(string content)
        {
            Console.WriteLine($"===> Context.UserIdentifier!: {Context.UserIdentifier!}");
            await Clients.User(Context.UserIdentifier!).ReceiveNotification(content);
        }

        /// <summary>
        /// Sends a location from the client.
        /// </summary>
        /// <param name="data">The location of current user.</param>
        /// <returns></returns>
        public async Task SendLocation(string data)
        {
            Console.WriteLine($"===> Current User: {Context.UserIdentifier!}");
            Console.WriteLine($"===> Data: {data}");

            var currentUserId = Guid.Parse(Context.UserIdentifier!);

            var currentUser = (await _userRepository.GetByIdAsync(currentUserId)).Value;

            var friendships = await _friendshipRepository.GetFriendshipsAsync(currentUser);

            var friendshipIds = friendships.Select(fr => fr.FriendId.ToString()).ToList();

            await _notificationsHubContext.Clients.User(Context.UserIdentifier!.ToString()).ReceiveLocation(data);

            await _notificationsHubContext.Clients.Users(friendshipIds).ReceiveLocation(data);
        }

        /// <summary>
        /// Trackes a location from the client.
        /// </summary>
        /// <param name="data">The data of the notification.</param>
        /// <returns></returns>
        public async Task TrackLocation(string data)
        {
            Console.WriteLine($"===> Data: {data}");
            await _producer.PublishAsync(MessageQueueConfiguration.SOS_TOPIC, data);

            var locationResponse = JsonSerializer.Deserialize<LocationResponse>(data, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });

            Console.WriteLine($"===> VictimId in SendLocation: {locationResponse!.VictimId}");
            Console.WriteLine($"===> Longitude in SendLocation: {locationResponse!.Longitude}");
            Console.WriteLine($"===> Latitude in SendLocation: {locationResponse!.Latitude}");

            var victim = (await _userRepository.GetByIdAsync(locationResponse!.VictimId)).Value;

            var friendships = await _friendshipRepository.GetFriendshipsAsync(victim);

            var friendshipIds = friendships.Select(fr => fr.FriendId.ToString()).ToList();

            var jsonContent = JsonSerializer.Serialize(
                new
                {
                    Title = "Thông báo cứu trợ khẩn cấp",
                    Avatar = victim.Avatar!.AvatarUrl,
                    Message = $"Bạn của bạn là {victim.FullName} đang cần được trợ giúp!."
                }
            );

            var cacheKey = $"{locationResponse.VictimId}_{DateTime.Now.Ticks}";

            var cacheData = await _cacheService.GetAsync(cacheKey);

            if (cacheData == null)
            {
                await _notificationsHubContext.Clients.Users(friendshipIds).ReceiveNotification(jsonContent);
            }

            await _cacheService.SetAsync(cacheKey, locationResponse, TimeSpan.FromDays(1));

            await _notificationsHubContext.Clients.Users(friendshipIds).TrackLocation(locationResponse);
        }

        /// <summary>
        /// Sends a safe from the victim.
        /// </summary>
        /// <returns></returns>
        public async Task SendSafeFromVicTim()
        {
            Console.WriteLine($"===> Victim!: {Context.UserIdentifier!}");

            var victimId = Guid.Parse(Context.UserIdentifier!);

            var victim = (await _userRepository.GetByIdAsync(victimId)).Value;

            var friendships = await _friendshipRepository.GetFriendshipsAsync(victim);

            var friendshipIds = friendships.Select(fr => fr.FriendId.ToString()).ToList();

            var jsonContent = JsonSerializer.Serialize(
                new
                {
                    Title = "Thông báo an toàn",
                    Avatar = victim.Avatar!.AvatarUrl,
                    Message = $"Bạn của bạn là {victim.FullName} đã an toàn!."
                }
            );

            await _notificationsHubContext.Clients.Users(friendshipIds).ReceiveNotification(jsonContent);

            await _consumer.UnsubscribeAsync(MessageQueueConfiguration.SOS_TOPIC, MessageQueueConfiguration.SOS_FRIENSHIP_GROUP);

            // await _cacheService.RemoveAsync(victimId.ToString());
        }

        /// <summary>
        /// Logs out the user.
        /// </summary>
        /// <returns></returns>
        public async Task Logout()
        {
            string userId = Context.UserIdentifier!;
            if (_usersConnected.Contains(userId))
            {
                _usersConnected.Remove(userId);
            }

            await Clients.All.DisconnectAsync(_usersConnected);
        }

        // <inheritdoc />
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            string userId = Context.UserIdentifier!;
            if (_usersConnected.Contains(userId))
            {
                _usersConnected.Remove(userId);
            }

            Clients.All.DisconnectAsync(_usersConnected);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
