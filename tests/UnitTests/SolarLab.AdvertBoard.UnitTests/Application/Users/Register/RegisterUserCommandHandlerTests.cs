using AutoFixture;
using FluentAssertions;
using Moq;
using SolarLab.AdvertBoard.Application.Abstractions;
using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Users.Register;
using SolarLab.AdvertBoard.Contracts.Users;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.UnitTests.Application.Users.Register
{
    public class RegisterUserCommandHandlerTests
    {
        private readonly Mock<IUserManagerProvider> _userManagerProviderMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly RegisterUserCommandHandler _handler;
        private readonly Fixture _fixture;

        public RegisterUserCommandHandlerTests()
        {
            _userManagerProviderMock = new Mock<IUserManagerProvider>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new RegisterUserCommandHandler(
                _userManagerProviderMock.Object,
                _userRepositoryMock.Object,
                _unitOfWorkMock.Object);

            _fixture = new Fixture();
        }

        [Fact]
        public async Task Handle_Should_Register_User_When_All_Data_Valid()
        {
            // Arrange
            var command = _fixture
               .Build<RegisterUserCommand>()
               .With(x => x.Email, "Test@mail.ru")
               .With(x => x.ContactEmail, "Test@mail.ru")
               .With(x => x.Password, "ValidPass123!")
               .With(x => x.FirstName, "John")
               .With(x => x.LastName, "Doe")
               .With(x => x.MiddleName, "Michael")
               .With(x => x.PhoneNumber, "+79039495566")
               .Create();

            var identityUserId = "auth0|123456789";
            _userManagerProviderMock
                .Setup(x => x.CreateIdentityUserAsync(command.Email, command.Password))
                .ReturnsAsync(Result.Success(identityUserId));

            _unitOfWorkMock
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeOfType<UserIdResponse>();

            _userManagerProviderMock.Verify(
                x => x.CreateIdentityUserAsync(command.Email, command.Password),
                Times.Once);

            _userRepositoryMock.Verify(
                x => x.Add(It.Is<User>(u =>
                    u.IdentityId == identityUserId &&
                    u.FirstName.Value == command.FirstName &&
                    u.LastName.Value == command.LastName &&
                    u.MiddleName!.Value == command.MiddleName &&
                    u.ContactEmail.Value == command.ContactEmail &&
                    u.PhoneNumber!.Value == command.PhoneNumber)),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Fail_When_FirstName_Invalid()
        {
            // Arrange
            var command = _fixture
               .Build<RegisterUserCommand>()
               .With(x => x.Email, "Test@mail.ru")
               .With(x => x.ContactEmail, "")
               .With(x => x.Password, "ValidPass123!")
               .With(x => x.FirstName, "John123")
               .With(x => x.LastName, "Doe")
               .With(x => x.MiddleName, "")
               .With(x => x.PhoneNumber, "")
               .Create();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.FirstName.NotValid);

            _userManagerProviderMock.Verify(
                x => x.CreateIdentityUserAsync(It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);

            _userRepositoryMock.Verify(
                x => x.Add(It.IsAny<User>()),
                Times.Never);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Fail_When_LastName_Invalid()
        {
            // Arrange
            var command = _fixture
               .Build<RegisterUserCommand>()
               .With(x => x.Email, "Test@mail.ru")
               .With(x => x.ContactEmail, "")
               .With(x => x.Password, "ValidPass123!")
               .With(x => x.FirstName, "John")
               .With(x => x.LastName, "Doe123")
               .With(x => x.MiddleName, "")
               .With(x => x.PhoneNumber, "")
               .Create();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.LastName.NotValid);

            _userManagerProviderMock.Verify(
                x => x.CreateIdentityUserAsync(It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);

            _userRepositoryMock.Verify(
                x => x.Add(It.IsAny<User>()),
                Times.Never);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Fail_When_ContactEmail_Invalid()
        {
            // Arrange
            var command = _fixture
                .Build<RegisterUserCommand>()
                .With(x => x.Email, "Test@mail.ru")
                .With(x => x.ContactEmail, "invalid-mail")
                .With(x => x.Password, "ValidPass123!")
                .With(x => x.FirstName, "John")
                .With(x => x.LastName, "Doe")
                .With(x => x.MiddleName, "")
                .With(x => x.PhoneNumber, "")
                .Create();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.Email.NotValid);

            _userManagerProviderMock.Verify(
                x => x.CreateIdentityUserAsync(It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);

            _userRepositoryMock.Verify(
                x => x.Add(It.IsAny<User>()),
                Times.Never);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Fail_When_PhoneNumber_Invalid()
        {
            // Arrange
            var command = _fixture
               .Build<RegisterUserCommand>()
               .With(x => x.Email, "Test@mail.ru")
               .With(x => x.ContactEmail, "")
               .With(x => x.Password, "ValidPass123!")
               .With(x => x.FirstName, "John")
               .With(x => x.LastName, "Doe")
               .With(x => x.MiddleName, "")
               .With(x => x.PhoneNumber, "invalid=phone")
               .Create();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.PhoneNumber.NotValid);

            _userManagerProviderMock.Verify(
                x => x.CreateIdentityUserAsync(It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);

            _userRepositoryMock.Verify(
                x => x.Add(It.IsAny<User>()),
                Times.Never);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Fail_When_IdentityService_Fails()
        {
            // Arrange
            var command = _fixture
               .Build<RegisterUserCommand>()
               .With(x => x.Email, "Test@mail.ru")
               .With(x => x.ContactEmail, "")
               .With(x => x.Password, "ValidPass123!")
               .With(x => x.FirstName, "John")
               .With(x => x.LastName, "Doe")
               .With(x => x.MiddleName, "")
               .With(x => x.PhoneNumber, "")
               .Create();

            var identityError = new Error("Identity.Error", "User already exists");
            _userManagerProviderMock
                .Setup(x => x.CreateIdentityUserAsync(command.Email, command.Password))
                .ReturnsAsync(Result.Failure<string>(identityError));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(identityError);

            _userRepositoryMock.Verify(
                x => x.Add(It.IsAny<User>()),
                Times.Never);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Use_Email_As_ContactEmail_When_ContactEmail_Empty()
        {
            // Arrange
            var command = _fixture
               .Build<RegisterUserCommand>()
               .With(x => x.Email, "Test@mail.ru")
               .With(x => x.ContactEmail, "")
               .With(x => x.Password, "ValidPass123!")
               .With(x => x.FirstName, "John")
               .With(x => x.LastName, "Doe")
               .With(x => x.MiddleName, "")
               .With(x => x.PhoneNumber, "")
               .Create();

            var identityUserId = "auth0|123456789";
            _userManagerProviderMock
                .Setup(x => x.CreateIdentityUserAsync(command.Email, command.Password))
                .ReturnsAsync(Result.Success(identityUserId));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();

            _userRepositoryMock.Verify(
                x => x.Add(It.Is<User>(u =>
                    u.ContactEmail.Value == command.Email)), 
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Create_User_With_Empty_Optional_Fields()
        {
            // Arrange
            var command = _fixture
               .Build<RegisterUserCommand>()
               .With(x => x.Email, "Test@mail.ru")
               .With(x => x.ContactEmail, "")
               .With(x => x.Password, "ValidPass123!")
               .With(x => x.FirstName, "John")
               .With(x => x.LastName, "Doe")
               .With(x => x.MiddleName, "")
               .With(x => x.PhoneNumber, "")
               .Create();

            var identityUserId = "auth0|123456789";
            _userManagerProviderMock
                .Setup(x => x.CreateIdentityUserAsync(command.Email, command.Password))
                .ReturnsAsync(Result.Success(identityUserId));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();

            _userRepositoryMock.Verify(
                x => x.Add(It.Is<User>(u =>
                    u.MiddleName == null &&
                    u.PhoneNumber == null)),
                Times.Once);
        }
    }
}
