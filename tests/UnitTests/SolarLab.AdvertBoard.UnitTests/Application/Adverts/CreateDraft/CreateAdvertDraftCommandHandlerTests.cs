using AutoFixture;
using FluentAssertions;
using Moq;
using SolarLab.AdvertBoard.Application.Abstractions;
using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Adverts.CreateDraft;
using SolarLab.AdvertBoard.Application.Users.Specifications;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Categories;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel.Maybe;

namespace SolarLab.AdvertBoard.UnitTests.Application.Adverts.CreateDraft
{
    public class CreateAdvertDraftCommandHandlerTests
    {
        private readonly Mock<IUserIdentifierProvider> _userIdentifierProviderMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IAdvertRepository> _advertRepositoryMock;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly CreateAdvertDraftCommandHandler _handler;
        private readonly Fixture _fixture;

        public CreateAdvertDraftCommandHandlerTests()
        {
            _userIdentifierProviderMock = new Mock<IUserIdentifierProvider>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _advertRepositoryMock = new Mock<IAdvertRepository>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new CreateAdvertDraftCommandHandler(
                _userIdentifierProviderMock.Object,
                _userRepositoryMock.Object,
                _advertRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _unitOfWorkMock.Object);

            _fixture = new Fixture();
        }

        [Fact]
        public async Task Handle_Should_Create_AdvertDraft_When_All_Data_Valid()
        {
            // Arrange
            var command = _fixture
               .Build<CreateAdvertDraftCommand>()
               .With(x => x.Title, "MacBook Pro 2023")
               .With(x => x.Description, "Excellent condition laptop")
               .With(x => x.Price, 1500.50m)
               .With(x => x.CategoryId, Guid.NewGuid())
               .Create();

            var identityId = "auth0|123456789";
            var user = CreateTestUser(identityId);
            var category = CreateTestCategory(command.CategoryId, canHostAdverts: true);

            _userIdentifierProviderMock
                .Setup(x => x.IdentityUserId)
                .Returns(identityId);

            _userRepositoryMock
                .Setup(x => x.GetBySpecificationAsync(It.IsAny<UserWithIdentitySpecification>()))
                .ReturnsAsync(user);

            _categoryRepositoryMock
                .Setup(x => x.GetByIdAsync(It.Is<CategoryId>(id => id.Id == command.CategoryId)))
                .ReturnsAsync(category);

            _unitOfWorkMock
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeOfType<AdvertIdResponse>();

            _advertRepositoryMock.Verify(
                x => x.Add(It.Is<Advert>(a =>
                    a.AuthorId == user.Id &&
                    a.CategoryId.Id == command.CategoryId &&
                    a.Title.Value == command.Title &&
                    a.Description.Value == command.Description &&
                    a.Price.Value == command.Price &&
                    a.Status == AdvertStatus.Draft)),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Fail_When_User_Not_Found()
        {
            // Arrange
            var command = _fixture
               .Build<CreateAdvertDraftCommand>()
               .With(x => x.Title, "MacBook Pro 2023")
               .With(x => x.Description, "Excellent condition laptop")
               .With(x => x.Price, 1500.50m)
               .With(x => x.CategoryId, Guid.NewGuid())
               .Create();

            var identityId = "auth0|123456789";

            _userIdentifierProviderMock
                .Setup(x => x.IdentityUserId)
                .Returns(identityId);

            _userRepositoryMock
                .Setup(x => x.GetBySpecificationAsync(It.IsAny<UserWithIdentitySpecification>()))
                .ReturnsAsync(Maybe<User>.None);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.NotFound);

            _advertRepositoryMock.Verify(
                x => x.Add(It.IsAny<Advert>()),
                Times.Never);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Fail_When_Category_Not_Found()
        {
            // Arrange
            var command = _fixture
               .Build<CreateAdvertDraftCommand>()
               .With(x => x.Title, "MacBook Pro 2023")
               .With(x => x.Description, "Excellent condition laptop")
               .With(x => x.Price, 1500.50m)
               .With(x => x.CategoryId, Guid.NewGuid())
               .Create();

            var identityId = "auth0|123456789";
            var user = CreateTestUser(identityId);

            _userIdentifierProviderMock
                .Setup(x => x.IdentityUserId)
                .Returns(identityId);

            _userRepositoryMock
                .Setup(x => x.GetBySpecificationAsync(It.IsAny<UserWithIdentitySpecification>()))
                .ReturnsAsync(user);

            _categoryRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<CategoryId>()))
                .ReturnsAsync(Maybe<Category>.None);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CategoryErrors.NotFound);

            _advertRepositoryMock.Verify(
                x => x.Add(It.IsAny<Advert>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Fail_When_Category_Cannot_Host_Adverts()
        {
            // Arrange
            var command = _fixture
               .Build<CreateAdvertDraftCommand>()
               .With(x => x.Title, "MacBook Pro 2023")
               .With(x => x.Description, "Excellent condition laptop")
               .With(x => x.Price, 1500.50m)
               .With(x => x.CategoryId, Guid.NewGuid())
               .Create();

            var identityId = "auth0|123456789";
            var user = CreateTestUser(identityId);
            var category = CreateTestCategory(command.CategoryId, canHostAdverts: false); 

            _userIdentifierProviderMock
                .Setup(x => x.IdentityUserId)
                .Returns(identityId);

            _userRepositoryMock
                .Setup(x => x.GetBySpecificationAsync(It.IsAny<UserWithIdentitySpecification>()))
                .ReturnsAsync(user);

            _categoryRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<CategoryId>()))
                .ReturnsAsync(category);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CategoryErrors.CantHostAdverts);

            _advertRepositoryMock.Verify(
                x => x.Add(It.IsAny<Advert>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Fail_When_Title_Invalid()
        {
            // Arrange
            var command = _fixture
               .Build<CreateAdvertDraftCommand>()
               .With(x => x.Title, "")
               .With(x => x.Description, "Excellent condition laptop")
               .With(x => x.Price, 1500.50m)
               .With(x => x.CategoryId, Guid.NewGuid())
               .Create();

            var identityId = "auth0|123456789";
            var user = CreateTestUser(identityId);
            var category = CreateTestCategory(command.CategoryId, canHostAdverts: true);

            _userIdentifierProviderMock
                .Setup(x => x.IdentityUserId)
                .Returns(identityId);

            _userRepositoryMock
                .Setup(x => x.GetBySpecificationAsync(It.IsAny<UserWithIdentitySpecification>()))
                .ReturnsAsync(user);

            _categoryRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<CategoryId>()))
                .ReturnsAsync(category);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AdvertErrors.Title.Empty);

            _advertRepositoryMock.Verify(
                x => x.Add(It.IsAny<Advert>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Fail_When_Description_Invalid()
        {
            // Arrange
            var command = _fixture
               .Build<CreateAdvertDraftCommand>()
               .With(x => x.Title, "MacBook Pro 2023")
               .With(x => x.Description, "E")
               .With(x => x.Price, 1500.50m)
               .With(x => x.CategoryId, Guid.NewGuid())
               .Create();

            var identityId = "auth0|123456789";
            var user = CreateTestUser(identityId);
            var category = CreateTestCategory(command.CategoryId, canHostAdverts: true);

            _userIdentifierProviderMock
                .Setup(x => x.IdentityUserId)
                .Returns(identityId);

            _userRepositoryMock
                .Setup(x => x.GetBySpecificationAsync(It.IsAny<UserWithIdentitySpecification>()))
                .ReturnsAsync(user);

            _categoryRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<CategoryId>()))
                .ReturnsAsync(category);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AdvertErrors.Description.TooShort);

            _advertRepositoryMock.Verify(
                x => x.Add(It.IsAny<Advert>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Fail_When_Price_Invalid()
        {
            // Arrange
            var command = _fixture
               .Build<CreateAdvertDraftCommand>()
               .With(x => x.Title, "MacBook Pro 2023")
               .With(x => x.Description, "Excellent condition laptop")
               .With(x => x.Price, -1500.50m)
               .With(x => x.CategoryId, Guid.NewGuid())
               .Create();

            var identityId = "auth0|123456789";
            var user = CreateTestUser(identityId);
            var category = CreateTestCategory(command.CategoryId, canHostAdverts: true);

            _userIdentifierProviderMock
                .Setup(x => x.IdentityUserId)
                .Returns(identityId);

            _userRepositoryMock
                .Setup(x => x.GetBySpecificationAsync(It.IsAny<UserWithIdentitySpecification>()))
                .ReturnsAsync(user);

            _categoryRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<CategoryId>()))
                .ReturnsAsync(category);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AdvertErrors.Price.TooLow);

            _advertRepositoryMock.Verify(
                x => x.Add(It.IsAny<Advert>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Return_AdvertId_Response()
        {
            // Arrange
            var command = _fixture
               .Build<CreateAdvertDraftCommand>()
               .With(x => x.Title, "MacBook Pro 2023")
               .With(x => x.Description, "Excellent condition laptop")
               .With(x => x.Price, 1500.50m)
               .With(x => x.CategoryId, Guid.NewGuid())
               .Create();

            var identityId = "auth0|123456789";
            var user = CreateTestUser(identityId);
            var category = CreateTestCategory(command.CategoryId, canHostAdverts: true);

            _userIdentifierProviderMock
                .Setup(x => x.IdentityUserId)
                .Returns(identityId);

            _userRepositoryMock
                .Setup(x => x.GetBySpecificationAsync(It.IsAny<UserWithIdentitySpecification>()))
                .ReturnsAsync(user);

            _categoryRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<CategoryId>()))
                .ReturnsAsync(category);

            Advert capturedAdvert = null;
            _advertRepositoryMock
                .Setup(x => x.Add(It.IsAny<Advert>()))
                .Callback<Advert>(a => capturedAdvert = a);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Id.Should().Be(capturedAdvert.Id);
        }

        private static User CreateTestUser(string identityId)
        {
            var firstName = FirstName.Create("John").Value;
            var lastName = LastName.Create("Doe").Value;
            var contactEmail = ContactEmail.Create("john.doe@example.com").Value;

            return User.Create(identityId, firstName, lastName, null, contactEmail, null);
        }

        private static Category CreateTestCategory(Guid categoryId, bool canHostAdverts)
        {
            var category = Category.CreateRoot(CategoryTitle.Create("Electronics").Value);

            if (!canHostAdverts)
            {
                category.AddChild(CategoryTitle.Create("Smartphones").Value);
            }

            return category;
        }
    }
}
