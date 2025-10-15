using FluentAssertions;
using Moq;
using SolarLab.AdvertBoard.Application.Abstractions;
using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Adverts.PublishDraft;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Categories;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel.Maybe;

namespace SolarLab.AdvertBoard.UnitTests.Application.Adverts.PublishDraft
{
    public class PublishAdvertDraftCommandHandlerTests
    {
        private readonly Mock<IAdvertRepository> _advertRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserIdentifierProvider> _userIdentifierProviderMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly PublishAdvertDraftCommandHandler _handler;

        public PublishAdvertDraftCommandHandlerTests()
        {
            _advertRepositoryMock = new Mock<IAdvertRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userIdentifierProviderMock = new Mock<IUserIdentifierProvider>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new PublishAdvertDraftCommandHandler(
                _advertRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _userIdentifierProviderMock.Object,
                _userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Publish_Draft_When_User_Is_Owner()
        {
            // Arrange
            var command = new PublishAdvertDraftCommand(Guid.NewGuid());
            var draftAdvert = CreateTestDraftAdvert();
            var currentUserId = "auth0|current-user";

            _advertRepositoryMock
                .Setup(x => x.GetByIdAsync(It.Is<AdvertId>(id => id.Id == command.Id)))
                .ReturnsAsync(draftAdvert);

            _userIdentifierProviderMock
                .Setup(x => x.IdentityUserId)
                .Returns(currentUserId);

            _userRepositoryMock
                .Setup(x => x.IsOwner(It.Is<UserId>(id => id.Id == draftAdvert.AuthorId.Id), currentUserId))
                .ReturnsAsync(true);

            _unitOfWorkMock
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();

            draftAdvert.Status.Should().Be(AdvertStatus.Published);
            draftAdvert.PublishedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            draftAdvert.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Fail_When_Advert_Not_Found()
        {
            // Arrange
            var command = new PublishAdvertDraftCommand(Guid.NewGuid());

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
            var command = new PublishAdvertDraftCommand(Guid.NewGuid());
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
        public async Task Handle_Should_Use_Correct_Advert_Id()
        {
            // Arrange
            var expectedAdvertId = Guid.NewGuid();
            var command = new PublishAdvertDraftCommand(expectedAdvertId);
            var draftAdvert = CreateTestDraftAdvert();
            var currentUserId = "auth0|current-user";

            _advertRepositoryMock
                .Setup(x => x.GetByIdAsync(It.Is<AdvertId>(id => id.Id == expectedAdvertId)))
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

            _advertRepositoryMock.Verify(
                x => x.GetByIdAsync(It.Is<AdvertId>(id => id.Id == expectedAdvertId)),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Use_Correct_User_For_Ownership_Check()
        {
            // Arrange
            var command = new PublishAdvertDraftCommand(Guid.NewGuid());
            var draftAdvert = CreateTestDraftAdvert();
            var expectedCurrentUserId = "auth0|specific-user";

            _advertRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<AdvertId>()))
                .ReturnsAsync(draftAdvert);

            _userIdentifierProviderMock
                .Setup(x => x.IdentityUserId)
                .Returns(expectedCurrentUserId);

            _userRepositoryMock
                .Setup(x => x.IsOwner(It.Is<UserId>(id => id.Id == draftAdvert.AuthorId.Id), expectedCurrentUserId))
                .ReturnsAsync(true);

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
            var title = AdvertTitle.Create("Test Title").Value;
            var description = AdvertDescription.Create("Test Description").Value;
            var price = Price.Create(100m).Value;

            return Advert.CreateDraft(authorId, categoryId, title, description, price);
        }
    }
}
