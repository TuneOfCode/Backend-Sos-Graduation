namespace Sos.Contracts.Notifications
{
    /// <summary>
    /// Represents the notification request.
    /// </summary>
    public record NotificationRequest(
        Guid NotificationId,
        string Title,
        string Content,
        string ThumbnailUrl,
        string CreatedAt
    );
}
