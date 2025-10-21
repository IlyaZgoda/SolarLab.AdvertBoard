using SolarLab.AdvertBoard.Contracts.Mails;

namespace SolarLab.AdvertBoard.Application.Abstractions.Emails
{
    /// <summary>
    /// Сервис для отправки электронных писем.
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Отправляет электронное письмо.
        /// </summary>
        /// <param name="mailRequest">Запрос на отправку письма с данными получателя и содержимым.</param>
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
