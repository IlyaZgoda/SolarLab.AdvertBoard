using SolarLab.AdvertBoard.Contracts.Mails;

namespace SolarLab.AdvertBoard.Application.Abstractions.Notifications
{
    public interface IEmailNotificationSender
    {
        Task SendConfirmationEmail(ConfirmationEmail confirmationEmail);
    }
}
