using FluentAssertions;
using Moq;
using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Links;
using SolarLab.AdvertBoard.Application.Abstractions.Notifications;
using SolarLab.AdvertBoard.Application.Users.Register;
using SolarLab.AdvertBoard.Contracts.Links;
using SolarLab.AdvertBoard.Contracts.Mails;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.Domain.Users.Events;

namespace SolarLab.AdvertBoard.UnitTests.Application.Users.Register
{
    public class SendConfirmationLinkOnUserRegisteredDomainEventHandlerTests
    {
        private readonly Mock<IUserManagerProvider> _userManagerProviderMock;
        private readonly Mock<IEmailNotificationSender> _emailNotificationSenderMock;
        private readonly Mock<IUriGenerator> _uriGeneratorMock;
        private readonly SendConfirmationLinkOnUserRegisteredDomainEventHandler _handler;

        public SendConfirmationLinkOnUserRegisteredDomainEventHandlerTests()
        {
            _userManagerProviderMock = new Mock<IUserManagerProvider>();
            _emailNotificationSenderMock = new Mock<IEmailNotificationSender>();
            _uriGeneratorMock = new Mock<IUriGenerator>();
            _handler = new SendConfirmationLinkOnUserRegisteredDomainEventHandler(
                _userManagerProviderMock.Object,
                _emailNotificationSenderMock.Object,
                _uriGeneratorMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Send_Confirmation_Email_When_Event_Received()
        {
            // Arrange
            var domainEvent = new UserRegisteredDomainEvent(
                new UserId(Guid.NewGuid()),
                "auth0|123456789");

            var userEmail = "test@example.com";
            var confirmationToken = "email-confirmation-token";
            var confirmationUri = "https://example.com/confirm-email?token=abc123";

            _userManagerProviderMock
                .Setup(x => x.GetEmailByIdAsync(domainEvent.IdentityId))
                .ReturnsAsync(userEmail);

            _userManagerProviderMock
                .Setup(x => x.GenerateEmailConfirmationTokenAsync(userEmail))
                .ReturnsAsync(confirmationToken);

            _uriGeneratorMock
                .Setup(x => x.GenerateEmailConfirmationUri(It.Is<ConfirmationUriRequest>(
                    r => r.UserId == domainEvent.IdentityId && r.Token == confirmationToken)))
                .Returns(confirmationUri);

            // Act
            await _handler.Handle(domainEvent, CancellationToken.None);

            // Assert
            _userManagerProviderMock.Verify(
                x => x.GetEmailByIdAsync(domainEvent.IdentityId),
                Times.Once);

            _userManagerProviderMock.Verify(
                x => x.GenerateEmailConfirmationTokenAsync(userEmail),
                Times.Once);

            _uriGeneratorMock.Verify(
                x => x.GenerateEmailConfirmationUri(It.Is<ConfirmationUriRequest>(
                    r => r.UserId == domainEvent.IdentityId && r.Token == confirmationToken)),
                Times.Once);

            _emailNotificationSenderMock.Verify(
                x => x.SendConfirmationEmail(It.Is<ConfirmationEmail>(e =>
                    e.To == userEmail && e.Uri == confirmationUri)),
                Times.Once);
        }
    }
}
