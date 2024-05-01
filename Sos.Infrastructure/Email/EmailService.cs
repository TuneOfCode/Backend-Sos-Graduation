using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Sos.Application.Core.Abstractions.Email;
using Sos.Contracts.Email;
using Sos.Infrastructure.Email.Settings;

namespace Sos.Infrastructure.Email
{
    public sealed class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;

        public EmailService(IOptions<MailSettings> maiLSettingsOptions)
        {
            _mailSettings = maiLSettingsOptions.Value;
        }

        public async Task SendEmailAsync(MailRequest request)
        {
            var email = new MimeMessage
            {
                From =
                {
                    new MailboxAddress(_mailSettings.SenderDisplayName, _mailSettings.SenderEmail)
                },
                To =
                {
                    MailboxAddress.Parse(request.To)
                },
                Subject = request.Subject,
                Body = new TextPart(TextFormat.Html)
                {
                    Text = $"""
                        <div style=\"font-family: Helvetica,Arial,sans-serif;min-width:1000px;overflow:auto;line-height:2\">
                            <div style=\"margin:50px auto;width:70%;padding:20px 0\">
                                <p style=\"font-size:1.1em;\">📢 Xin chào, đây là dịch vụ mail của hệ thống SOS!</p>
                                <p>😄 Cảm ơn bạn đã sử dụng ứng dụng SOS này.</p>
                                    <p style=\"font-size:1.1em;\">
                                    🔔 Mã xác thực tài khoản của bạn là:  
                                    <span style=\"font-size:1.5em;background: #00466a;margin: 0 auto;width: max-content;padding: 0 10px;color: #fff;border-radius: 4px;\">
                                        <b>{request.Body}</b>
                                    </span>
                                </p>
                                <p style=\"font-size:0.9em;color: red; text-align: center; font-weight: bold; font-size: 0.8em\">
                                   📌 Lưu ý: Thời hạn của mã xác thực này là <b>3 phút ⏱</b>.
                                </p>
                                <p style=\"font-size:0.9em;\">
                                    Trân trọng,
                                    <br />
                                    <br />
                                    <b>Hệ thống SOS.</b>
                                </p>
                                <hr style=\"border:none;border-top:1px solid #eee\" />
                                <div style=\"float:right;padding:8px 0;color:#aaa;font-size:0.8em;line-height:1;font-weight:300\"></div>
                            </div>
                        </div>
                    """
                }
            };

            using var smptClient = new SmtpClient();

            await smptClient.ConnectAsync(
                _mailSettings.SmtpHost,
                _mailSettings.SmtpPort,
                SecureSocketOptions.StartTls
            );

            await smptClient.AuthenticateAsync(
                _mailSettings.SenderEmail,
                _mailSettings.SmtpPassword
            );

            await smptClient.SendAsync(email);

            await smptClient.DisconnectAsync(true);
        }
    }
}
