using AutoFixture;
using FluentAssertions;
using Moq;
using SolarLab.AdvertBoard.Application.Abstractions;
using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Adverts.CreateDraft;
using SolarLab.AdvertBoard.Application.Adverts.UpdateDraft;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Categories;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel.Maybe;

namespace SolarLab.AdvertBoard.UnitTests.Application.Adverts.UpdateDraft
{
    public class UpdateAdvertDraftCommandHandlerTests
    {
        private readonly Mock<IUserIdentifierProvider> _userIdentifierProviderMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IAdvertRepository> _advertRepositoryMock;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly UpdateAdvertDraftCommandHandler _handler;
        private readonly Fixture _fixture;

        public UpdateAdvertDraftCommandHandlerTests()
        {
            _userIdentifierProviderMock = new Mock<IUserIdentifierProvider>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _advertRepositoryMock = new Mock<IAdvertRepository>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new UpdateAdvertDraftCommandHandler(
                _userIdentifierProviderMock.Object,
                _userRepositoryMock.Object,
                _advertRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _unitOfWorkMock.Object);

            _fixture = new Fixture();
        }

        [Fact]
        public async Task Handle_Should_Update_Draft_When_All_Data_Valid()
        {
            // Arrange
            var command = _fixture
               .Build<UpdateAdvertDraftCommand>()
               .With(x => x.DraftId, Guid.NewGuid())
               .With(x => x.Title, "Updated Title")
               .With(x => x.Description, "Updated description text")
               .With(x => x.Price, 100.50m)
               .With(x => x.CategoryId, Guid.NewGuid())
               .Create();

            var draftAdvert = CreateTestDraftAdvert();
            var currentUserId = "auth0|current-user";
            var category = CreateTestCategory(command.CategoryId!.Value, canHostAdverts: true);

            _advertRepositoryMock
                .Setup(x => x.GetByIdAsync(It.Is<AdvertId>(id => id.Id == command.DraftId)))
                .ReturnsAsync(draftAdvert);

            _userIdentifierProviderMock
                .Setup(x => x.IdentityUserId)
                .Returns(currentUserId);

            _userRepositoryMock
                .Setup(x => x.IsOwner(It.Is<UserId>(id => id.Id == draftAdvert.AuthorId.Id), currentUserId))
                .ReturnsAsync(true);

            _categoryRepositoryMock
                .Setup(x => x.GetByIdAsync(It.Is<CategoryId>(id => id.Id == command.CategoryId.Value)))
                .ReturnsAsync(category);

            _unitOfWorkMock
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Update_Partial_Fields_When_Some_Data_Null()
        {
            // Arrange
            var command = new UpdateAdvertDraftCommand(Guid.NewGuid(), null, "Updated Title Only", null, null);

            var draftAdvert = CreateTestDraftAdvert();
            var currentUserId = "auth0|current-user";

            _advertRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<AdvertId>()))
                .ReturnsAsync(draftAdvert);

            _userIdentifierProviderMock
                .Setup(x => x.IdentityUserId)
                .Returns(currentUserId);

            _userRepositoryMock
                .Setup(x => x.IsOwner(It.IsAny<UserId>(), currentUserId))
                .ReturnsAsync(true);

            _unitOfWorkMock
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();

            _categoryRepositoryMock.Verify(
                x => x.GetByIdAsync(It.IsAny<CategoryId>()),
                Times.Never); 
        }

        [Fact]
        public async Task Handle_Should_Fail_When_Advert_Not_Found()
        {
            // Arrange
            var command = _fixture
               .Build<UpdateAdvertDraftCommand>()
               .With(x => x.DraftId, Guid.NewGuid())
               .With(x => x.Title, "Updated Title")
               .With(x => x.Description, "Updated description text")
               .With(x => x.Price, 100.50m)
               .With(x => x.CategoryId, Guid.NewGuid())
               .Create();

            _advertRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<AdvertId>()))
                .ReturnsAsync(Maybe<Advert>.None);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AdvertErrors.NotFound);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Fail_When_User_Not_Owner()
        {
            // Arrange
            var command = _fixture
               .Build<UpdateAdvertDraftCommand>()
               .With(x => x.DraftId, Guid.NewGuid())
               .With(x => x.Title, "Updated Title")
               .With(x => x.Description, "Updated description text")
               .With(x => x.Price, 100.50m)
               .With(x => x.CategoryId, Guid.NewGuid())
               .Create();

            var draftAdvert = CreateTestDraftAdvert();
            var differentUserId = "auth0|different-user";

            _advertRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<AdvertId>()))
                .ReturnsAsync(draftAdvert);

            _userIdentifierProviderMock
                .Setup(x => x.IdentityUserId)
                .Returns(differentUserId);

            _userRepositoryMock
                .Setup(x => x.IsOwner(It.IsAny<UserId>(), differentUserId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AdvertErrors.NotFound);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Fail_When_Category_Not_Found()
        {
            // Arrange
            var command = _fixture
               .Build<UpdateAdvertDraftCommand>()
               .With(x => x.DraftId, Guid.NewGuid())
               .With(x => x.Title, "Updated Title")
               .With(x => x.Description, "Updated description text")
               .With(x => x.Price, 100.50m)
               .With(x => x.CategoryId, Guid.NewGuid())
               .Create();

            var draftAdvert = CreateTestDraftAdvert();
            var currentUserId = "auth0|current-user";

            _advertRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<AdvertId>()))
                .ReturnsAsync(draftAdvert);

            _userIdentifierProviderMock
                .Setup(x => x.IdentityUserId)
                .Returns(currentUserId);

            _userRepositoryMock
                .Setup(x => x.IsOwner(It.IsAny<UserId>(), currentUserId))
                .ReturnsAsync(true);

            _categoryRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<CategoryId>()))
                .ReturnsAsync(Maybe<Category>.None);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CategoryErrors.NotFound);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Fail_When_Category_Cannot_Host_Adverts()
        {
            // Arrange
            var command = _fixture
               .Build<UpdateAdvertDraftCommand>()
               .With(x => x.DraftId, Guid.NewGuid())
               .With(x => x.Title, "Updated Title")
               .With(x => x.Description, "Updated description text")
               .With(x => x.Price, 100.50m)
               .With(x => x.CategoryId, Guid.NewGuid())
               .Create();

            var draftAdvert = CreateTestDraftAdvert();
            var currentUserId = "auth0|current-user";
            var category = CreateTestCategory(command.CategoryId!.Value, canHostAdverts: false);

            _advertRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<AdvertId>()))
                .ReturnsAsync(draftAdvert);

            _userIdentifierProviderMock
                .Setup(x => x.IdentityUserId)
                .Returns(currentUserId);

            _userRepositoryMock
                .Setup(x => x.IsOwner(It.IsAny<UserId>(), currentUserId))
                .ReturnsAsync(true);

            _categoryRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<CategoryId>()))
                .ReturnsAsync(category);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CategoryErrors.CantHostAdverts);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Fail_When_Title_Invalid()
        {
            // Arrange
            var command = _fixture
               .Build<UpdateAdvertDraftCommand>()
               .With(x => x.DraftId, Guid.NewGuid())
               .With(x => x.Title, "")
               .With(x => x.Description, "Updated description text")
               .With(x => x.Price, 100.50m)
               .With(x => x.CategoryId, Guid.NewGuid())
               .Create();

            var draftAdvert = CreateTestDraftAdvert();
            var currentUserId = "auth0|current-user";
            var category = CreateTestCategory(command.CategoryId.Value, canHostAdverts: true);

            _advertRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<AdvertId>()))
                .ReturnsAsync(draftAdvert);

            _userIdentifierProviderMock
                .Setup(x => x.IdentityUserId)
                .Returns(currentUserId);

            _userRepositoryMock
                .Setup(x => x.IsOwner(It.IsAny<UserId>(), currentUserId))
                .ReturnsAsync(true);

            _categoryRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<CategoryId>()))
                .ReturnsAsync(category);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AdvertErrors.Title.Empty);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Fail_When_Description_Invalid()
        {
            // Arrange
            var command = _fixture
               .Build<UpdateAdvertDraftCommand>()
               .With(x => x.DraftId, Guid.NewGuid())
               .With(x => x.Title, "Updated Title")
               .With(x => x.Description, "Up")
               .With(x => x.Price, 100.50m)
               .With(x => x.CategoryId, Guid.NewGuid())
               .Create();

            var draftAdvert = CreateTestDraftAdvert();
            var currentUserId = "auth0|current-user";
            var category = CreateTestCategory(command.CategoryId!.Value, canHostAdverts: true);

            _advertRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<AdvertId>()))
                .ReturnsAsync(draftAdvert);

            _userIdentifierProviderMock
                .Setup(x => x.IdentityUserId)
                .Returns(currentUserId);

            _userRepositoryMock
                .Setup(x => x.IsOwner(It.IsAny<UserId>(), currentUserId))
                .ReturnsAsync(true);

            _categoryRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<CategoryId>()))
                .ReturnsAsync(category);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AdvertErrors.Description.TooShort);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Fail_When_Price_Invalid()
        {
            // Arrange
            var command = _fixture
               .Build<UpdateAdvertDraftCommand>()
               .With(x => x.DraftId, Guid.NewGuid())
               .With(x => x.Title, "Updated Title")
               .With(x => x.Description, "Updated description text")
               .With(x => x.Price, -100.50m)
               .With(x => x.CategoryId, Guid.NewGuid())
               .Create();

            var draftAdvert = CreateTestDraftAdvert();
            var currentUserId = "auth0|current-user";
            var category = CreateTestCategory(command.CategoryId!.Value, canHostAdverts: true);

            _advertRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<AdvertId>()))
                .ReturnsAsync(draftAdvert);

            _userIdentifierProviderMock
                .Setup(x => x.IdentityUserId)
                .Returns(currentUserId);

            _userRepositoryMock
                .Setup(x => x.IsOwner(It.IsAny<UserId>(), currentUserId))
                .ReturnsAsync(true);

            _categoryRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<CategoryId>()))
                .ReturnsAsync(category);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AdvertErrors.Price.TooLow);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }
  
        [Fact]
        public async Task Handle_Should_Use_Correct_User_For_Ownership_Check()
        {
            // Arrange
            var command = _fixture
               .Build<UpdateAdvertDraftCommand>()
               .With(x => x.DraftId, Guid.NewGuid())
               .With(x => x.Title, "Updated Title")
               .With(x => x.Description, "Updated description text")
               .With(x => x.Price, 100.50m)
               .With(x => x.CategoryId, Guid.NewGuid())
               .Create();

            var draftAdvert = CreateTestDraftAdvert();
            var expectedCurrentUserId = "auth0|specific-user";
            var category = CreateTestCategory(command.CategoryId!.Value, canHostAdverts: true);

            _advertRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<AdvertId>()))
                .ReturnsAsync(draftAdvert);

            _userIdentifierProviderMock
                .Setup(x => x.IdentityUserId)
                .Returns(expectedCurrentUserId);

            _userRepositoryMock
                .Setup(x => x.IsOwner(It.Is<UserId>(id => id.Id == draftAdvert.AuthorId.Id), expectedCurrentUserId))
                .ReturnsAsync(true);

            _categoryRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<CategoryId>()))
                .ReturnsAsync(category);

            _unitOfWorkMock
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();

            _userRepositoryMock.Verify(
                x => x.IsOwner(It.Is<UserId>(id => id.Id == draftAdvert.AuthorId.Id), expectedCurrentUserId),
                Times.Once);
        }

        private static Advert CreateTestDraftAdvert()
        {
            var authorId = new UserId(Guid.NewGuid());
            var categoryId = new CategoryId(Guid.NewGuid());
            var title = AdvertTitle.Create("Original Title").Value;
            var description = AdvertDescription.Create("Original description").Value;
            var price = Price.Create(100m).Value;

            return Advert.CreateDraft(authorId, categoryId, title, description, price);
        }

        private static Category CreateTestCategory(Guid categoryId, bool canHostAdverts)
        {
            var category = Category.CreateRoot(CategoryTitle.Create("Test Category").Value);

            if (!canHostAdverts)
            {
                category.AddChild(CategoryTitle.Create("Child Category").Value);
            }

            return category;
        }
    }
}
