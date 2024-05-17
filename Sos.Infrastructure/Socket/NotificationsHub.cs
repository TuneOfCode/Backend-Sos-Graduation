using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Sos.Application.Core.Abstractions.Cache;
using Sos.Application.Core.Abstractions.MessageQueue;
using Sos.Application.Core.Abstractions.Socket;
using Sos.Contracts.Notifications;
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
        private readonly ICacheService _cacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationsHub"/> class.
        /// </summary>
        /// <param name="producer">The producer.</param>
        /// <param name="consumer">The consumer.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="cacheService">The cache service.</param>
        public NotificationsHub(
            IProducer producer,
            IConsumer consumer,
            IUserRepository userRepository,
            ICacheService cacheService
        )
        {
            _producer = producer;
            _consumer = consumer;
            _userRepository = userRepository;
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
        /// <param name="ids">The ids of friends.</param>
        /// <returns></returns>
        public async Task SendLocation(string data, string ids)
        {
            Console.WriteLine($"===> Current User: {Context.UserIdentifier!}");
            Console.WriteLine($"===> Data: {data}");
            Console.WriteLine($"===> Ids: {ids}");

            var friendshipIds = JsonSerializer.Deserialize<IList<string>>(ids);

            await Clients.User(Context.UserIdentifier!.ToString()).ReceiveLocation(data);

            await Clients.Users(friendshipIds!).ReceiveLocation(data);
        }

        /// <summary>
        /// Trackes a location from the client.
        /// </summary>
        /// <param name="data">The data of the notification.</param>
        /// <param name="ids">The ids of friends.</param>
        /// <returns></returns>
        public async Task SendTrackingLocation(string data, string ids)
        {
            Console.WriteLine($"===> Data: {data}");
            Console.WriteLine($"===> Ids: {ids}");

            var locationResponse = JsonSerializer.Deserialize<LocationResponse>(data, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });

            Console.WriteLine($"===> VictimId in SendLocation: {locationResponse!.VictimId}");
            Console.WriteLine($"===> Longitude in SendLocation: {locationResponse!.Longitude}");
            Console.WriteLine($"===> Latitude in SendLocation: {locationResponse!.Latitude}");

            var friendshipIds = JsonSerializer.Deserialize<IList<string>>(ids);

            await Clients.User(locationResponse!.VictimId.ToString()).TrackLocation(data);

            await Clients.Users(friendshipIds!).TrackLocation(data);

            var sosKey = "sos_notification";

            var sosCached = await _cacheService.GetAsync(sosKey);

            Console.WriteLine($"===> sosCached: {sosCached}");

            if (sosCached == null)
            {
                var victim = (await _userRepository.GetByIdAsync(locationResponse!.VictimId)).Value;

                var newNotificationItem = new NotificationRequest(
                    Guid.NewGuid(),
                    "Thông báo cứu trợ khẩn cấp",
                    $"Bạn của bạn là {victim.FullName} đang cần được trợ giúp!",
                    victim.Avatar!.AvatarUrl,
                    DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                );

                var jsonContent = JsonSerializer.Serialize(newNotificationItem);

                await Clients.Users(friendshipIds!).ReceiveNotification(jsonContent);

                await _cacheService.SetAsync(sosKey, locationResponse!.VictimId, TimeSpan.FromSeconds(30));

                await SaveNotificationAsync(_cacheService, friendshipIds!, newNotificationItem);
            }

            await _producer.PublishAsync(MessageQueueConfiguration.SOS_TOPIC, data);
        }

        /// <summary>
        /// Sends a safe from the victim.
        /// </summary>
        /// <param name="ids">The ids of friends.</param>
        /// <returns></returns>
        public async Task SendSafeFromVictim(string ids)
        {
            var victimId = Guid.Parse(Context.UserIdentifier!);

            var victim = (await _userRepository.GetByIdAsync(victimId)).Value;

            var friendshipIds = JsonSerializer.Deserialize<IList<string>>(ids);

            var newNotificationItem = new NotificationRequest(
                Guid.NewGuid(),
                "Thông báo an toàn",
                $"Bạn của bạn là {victim.FullName} đã an toàn!",
                victim.Avatar!.AvatarUrl,
                DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            );

            var jsonContent = JsonSerializer.Serialize(newNotificationItem);

            await Clients.Users(friendshipIds!).ReceiveNotification(jsonContent);

            await Clients.Users(friendshipIds!).ReceiveSafeFromVictim(victimId.ToString());

            await _cacheService.RemoveAsync("sos_notification");

            await SaveNotificationAsync(_cacheService, friendshipIds!, newNotificationItem);

            await _consumer.UnsubscribeAsync(MessageQueueConfiguration.SOS_TOPIC, MessageQueueConfiguration.SOS_FRIENSHIP_GROUP);
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

        private static async Task SaveNotificationAsync(ICacheService cacheService, IList<string> friendshipIds, NotificationRequest newNotificationItem)
        {
            foreach (var friendshipId in friendshipIds)
            {
                var notificationKey = $"notification_{friendshipId}";

                var notificationsCached = await cacheService.GetAsync(notificationKey);

                var newNotifications = new List<NotificationRequest>();

                Console.WriteLine($"===> notificationsCached with {friendshipId}: {notificationsCached}");

                if (notificationsCached == null)
                {
                    newNotifications.Add(newNotificationItem);
                }
                else
                {
                    newNotifications = JsonSerializer
                        .Deserialize<List<NotificationRequest>>(notificationsCached, new JsonSerializerOptions {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        });

                    newNotifications!.Add(newNotificationItem);
                }
                
                await cacheService.SetAsync(notificationKey, newNotifications!, TimeSpan.FromMinutes(3));
            }
        }
    }
}
