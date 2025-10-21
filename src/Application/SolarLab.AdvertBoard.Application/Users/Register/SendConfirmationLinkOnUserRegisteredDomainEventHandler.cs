using MediatR;
using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Links;
using SolarLab.AdvertBoard.Application.Abstractions.Notifications;
using SolarLab.AdvertBoard.Contracts.Links;
using SolarLab.AdvertBoard.Contracts.Mails;
using SolarLab.AdvertBoard.Domain.Users.Events;

namespace SolarLab.AdvertBoard.Application.Users.Register
{
    /// <summary>
    /// Обработчик события регистрации пользователя.
    /// </summary>
    public class SendConfirmationLinkOnUserRegisteredDomainEventHandler(
        IUserManagerProvider identityService, 
        IEmailNotificationSender emailNotificationSender, 
        IUriGenerator uriGenerator) : INotificationHandler<UserRegisteredDomainEvent>
    {
        /// <summary>
        /// Обрабатывает событие регистрации пользователя.
        /// </summary>
        public async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
        {
            var email = await identityService.GetEmailByIdAsync(notification.IdentityId);

            var token = await identityService.GenerateEmailConfirmationTokenAsync(email);

            var uri = uriGenerator.GenerateEmailConfirmationUri(new ConfirmationUriRequest(notification.IdentityId, token));  

            await emailNotificationSender.SendConfirmationEmail(new ConfirmationEmail(email, uri));
        }
    }
}
