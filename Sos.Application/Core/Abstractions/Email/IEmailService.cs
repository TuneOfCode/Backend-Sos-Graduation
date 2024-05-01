using Sos.Contracts.Email;

namespace Sos.Application.Core.Abstractions.Email
{
    /// <summary>
    /// Represents the email service interface.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends the email with the content based on the specified mail request.
        /// </summary>
        /// <param name="request">The mail request.</param>
        /// <returns>The completed task.</returns>

        Task SendEmailAsync(MailRequest request);
    }
}
