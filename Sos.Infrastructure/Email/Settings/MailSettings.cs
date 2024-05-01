namespace Sos.Infrastructure.Email.Settings
{
    /// <summary>
    /// Represents the mail settings.
    /// </summary>
    public class MailSettings
    {
        public const string SettingsKey = "Mail";

        /// <summary>
        /// Gets the sender display name.
        /// </summary>
        public string? SenderDisplayName { get; set; }

        /// <summary>
        /// Gets the sender email.
        /// </summary>
        public string? SenderEmail { get; set; }

        /// <summary>
        /// Gets the smtp host.
        /// </summary>
        public string? SmtpHost { get; set; }

        /// <summary>
        /// Gets the smtp port.
        /// </summary>
        public int SmtpPort { get; set; }

        /// <summary>
        /// Gets the smtp password
        /// </summary>
        public string? SmtpPassword { get; set; }
    };
}
