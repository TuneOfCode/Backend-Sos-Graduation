namespace Sos.Contracts.Email
{
    /// <summary>
    /// Represents the mail request.
    /// </summary>
    public record MailRequest(
        string To,
        string Subject,
        string Body
    );
}
