using SolarLab.AdvertBoard.Application.Abstractions.Emails;
using SolarLab.AdvertBoard.Application.Abstractions.Notifications;
using SolarLab.AdvertBoard.Contracts.Mails;

namespace SolarLab.AdvertBoard.Infrastructure.Notifications
{
    public class EmailNotificationSender(IEmailSender emailSender) : IEmailNotificationSender
    {
        public async Task SendConfirmationEmail(ConfirmationEmail confirmationEmail)
        {
            var htmlBody = $@"
                <h1>Подтвердите email</h1>
                <p>Для завершения регистрации нажмите на кнопку ниже:</p>
                <p><a href=""{confirmationEmail.Uri}"" 
                      style=""display:inline-block;padding:10px 20px;
                             background-color:#007bff;color:#fff;
                             text-decoration:none;border-radius:5px;"">
                      Подтвердить Email
                </a></p>";

            var mailRequest = new MailRequest(confirmationEmail.To, "Welcome to AdvertBoard", htmlBody);

            await emailSender.SendEmailAsync(mailRequest);
        }
    }
}
