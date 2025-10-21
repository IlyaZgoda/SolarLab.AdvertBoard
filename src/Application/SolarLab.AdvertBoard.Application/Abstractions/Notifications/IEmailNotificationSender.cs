using SolarLab.AdvertBoard.Contracts.Mails;

namespace SolarLab.AdvertBoard.Application.Abstractions.Notifications
{
    /// <summary>
    /// Сервис для отправки уведомлений по электронной почте.
    /// </summary>
    public interface IEmailNotificationSender
    {
        /// <summary>
        /// Отправляет email для подтверждения учетной записи.
        /// </summary>
        /// <param name="confirmationEmail">Данные для отправки письма подтверждения.</param>
        Task SendConfirmationEmail(ConfirmationEmail confirmationEmail);
    }
}
