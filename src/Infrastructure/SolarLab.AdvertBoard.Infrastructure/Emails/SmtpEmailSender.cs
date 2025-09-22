using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using SolarLab.AdvertBoard.Application.Abstractions.Emails;
using SolarLab.AdvertBoard.Contracts.Mails;

namespace SolarLab.AdvertBoard.Infrastructure.Emails
{
    public class SmtpEmailSender(IOptions<SmtpOptions> options) : IEmailSender
    {
        private readonly SmtpOptions _options = options.Value;

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage
            {
                From =
                {
                    new MailboxAddress(_options.DisplayName, _options.From)
                },
                To =
                {
                    MailboxAddress.Parse(mailRequest.To)
                },
                Subject = mailRequest.Subject,
                Body = new TextPart(TextFormat.Html)
                {
                    Text = mailRequest.Body
                }
            };

            using var smtpClient = new SmtpClient();

            await smtpClient.ConnectAsync(_options.Host, _options.Port, SecureSocketOptions.StartTls);

            await smtpClient.AuthenticateAsync(_options.Username, _options.Password);

            await smtpClient.SendAsync(email);

            await smtpClient.DisconnectAsync(true);
        }
    }
}
