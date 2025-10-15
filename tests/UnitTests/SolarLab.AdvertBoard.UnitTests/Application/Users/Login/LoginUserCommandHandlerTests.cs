using FluentAssertions;
using Moq;
using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Users.Login;
using SolarLab.AdvertBoard.Contracts.Authentication;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using static SolarLab.AdvertBoard.Domain.Errors.UserErrors;

namespace SolarLab.AdvertBoard.UnitTests.Application.Users.Login
{
    public class LoginUserCommandHandlerTests
    {
        private readonly Mock<IUserManagerProvider> _userManagerMock;
        private readonly Mock<ITokenProvider> _tokenProviderMock;
        private readonly LoginUserCommandHandler _handler;

        public LoginUserCommandHandlerTests()
        {
            _userManagerMock = new Mock<IUserManagerProvider>();
            _tokenProviderMock = new Mock<ITokenProvider>();
            _handler = new LoginUserCommandHandler(
                _userManagerMock.Object,
                _tokenProviderMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_Jwt_When_Credentials_Valid_And_Email_Confirmed()
        {
            // Arrange
            var command = new LoginUserCommand("test@example.com", "ValidPass123!");

            var identityUserId = "auth0|123456789";
            var expectedToken = "jwt-token-123";

            _userManagerMock
                .Setup(x => x.ValidateIdentityUserAsync(command.Email, command.Password))
                .ReturnsAsync(Result.Success(identityUserId));

            _userManagerMock
                .Setup(x => x.IsEmailConfirmed(identityUserId))
                .ReturnsAsync(true);

            _tokenProviderMock
                .Setup(x => x.Create(identityUserId, command.Email))
                .Returns(expectedToken);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Token.Should().Be(expectedToken);

            _userManagerMock.Verify(
                x => x.ValidateIdentityUserAsync(command.Email, command.Password),
                Times.Once);

            _userManagerMock.Verify(
                x => x.IsEmailConfirmed(identityUserId),
                Times.Once);

            _tokenProviderMock.Verify(
                x => x.Create(identityUserId, command.Email),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Fail_When_Invalid_Credentials()
        {
            // Arrange
            var command = new LoginUserCommand("test@example.com", "WrongPassword");

            var identityError = new Error(ErrorTypes.ValidationError, "Invalid credentials");
            _userManagerMock
                .Setup(x => x.ValidateIdentityUserAsync(command.Email, command.Password))
                .ReturnsAsync(Result.Failure<string>(identityError));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(identityError);

            _userManagerMock.Verify(
                x => x.ValidateIdentityUserAsync(command.Email, command.Password),
                Times.Once);

            _userManagerMock.Verify(
                x => x.IsEmailConfirmed(It.IsAny<string>()),
                Times.Never);

            _tokenProviderMock.Verify(
                x => x.Create(It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Fail_When_Email_Not_Confirmed()
        {
            // Arrange
            var command = new LoginUserCommand("test@example.com", "ValidPass123!");

            var identityUserId = "auth0|123456789";

            _userManagerMock
                .Setup(x => x.ValidateIdentityUserAsync(command.Email, command.Password))
                .ReturnsAsync(Result.Success(identityUserId));

            _userManagerMock
                .Setup(x => x.IsEmailConfirmed(identityUserId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be(ErrorTypes.ValidationError);
            result.Error.Description.Should().Be("Email is not confirmed");

            _userManagerMock.Verify(
                x => x.ValidateIdentityUserAsync(command.Email, command.Password),
                Times.Once);

            _userManagerMock.Verify(
                x => x.IsEmailConfirmed(identityUserId),
                Times.Once);

            _tokenProviderMock.Verify(
                x => x.Create(It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
        }


        [Theory]
        [InlineData("user@example.com")]
        [InlineData("user.name@example.com")]
        [InlineData("user+tag@example.com")]
        [InlineData("user@sub.domain.com")]
        public async Task Handle_Should_Work_With_Different_Email_Formats(string email)
        {
            var command = new LoginUserCommand(email, "ValidPass123!");

            var identityUserId = $"auth0|{Guid.NewGuid()}";
            var token = "jwt-token";

            _userManagerMock
                .Setup(x => x.ValidateIdentityUserAsync(email, command.Password))
                .ReturnsAsync(Result.Success(identityUserId));

            _userManagerMock
                .Setup(x => x.IsEmailConfirmed(identityUserId))
                .ReturnsAsync(true);

            _tokenProviderMock
                .Setup(x => x.Create(identityUserId, email))
                .Returns(token);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Token.Should().Be(token);
        }
    }
}
