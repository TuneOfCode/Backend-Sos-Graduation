namespace Sos.Contracts.Notifications
{
    /// <summary>
    /// Represents the notification response.
    /// </summary>
    public record NotificationResponse(
        Guid NotificationId,
        string Title,
        string Content,
        string ThumbnailUrl,
        String CreatedAt
    );
}
