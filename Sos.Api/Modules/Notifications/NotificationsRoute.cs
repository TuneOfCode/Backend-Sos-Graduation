namespace Sos.Api.Modules.Notifications
{
    /// <summary>
    /// Contains all routes for <see cref="NotificationsController"/>
    /// </summary>
    public static class NotificationsRoute
    {
        public const string GetNotificationsByUserId = "notifications/{userId:guid}";
    }
}
