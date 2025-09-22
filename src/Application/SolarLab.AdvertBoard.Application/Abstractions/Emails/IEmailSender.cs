using SolarLab.AdvertBoard.Contracts.Mails;

namespace SolarLab.AdvertBoard.Application.Abstractions.Emails
{
    public interface IEmailSender
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
