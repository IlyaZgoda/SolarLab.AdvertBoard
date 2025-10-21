using FluentAssertions;
using Moq;
using SolarLab.AdvertBoard.Application.Abstractions;
using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Adverts.DeletePublished;
using SolarLab.AdvertBoard.Domain.AdvertImages;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Categories;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel.Maybe;

namespace SolarLab.AdvertBoard.UnitTests.Application.Adverts.DeletePublish
{
    public class DeletePublishedAdvertCommandHandlerTests
    {
        private readonly Mock<IAdvertRepository> _advertRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserIdentifierProvider> _userIdentifierProviderMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly DeletePublishedAdvertCommandHandler _handler;

        public DeletePublishedAdvertCommandHandlerTests()
        {
            _advertRepositoryMock = new Mock<IAdvertRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userIdentifierProviderMock = new Mock<IUserIdentifierProvider>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new DeletePublishedAdvertCommandHandler(
                _advertRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _userIdentifierProviderMock.Object,
                _userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Delete_When_User_Is_Owner_And_Advert_Published()
        {
            // Arrange
            var command = new DeletePublishedAdvertCommand(Guid.NewGuid());
            var publishedAdvert = CreateTestPublishedAdvert();
            var currentUserId = "auth0|current-user";

            _advertRepositoryMock
                .Setup(x => x.GetByIdAsync(It.Is<AdvertId>(id => id.Id == command.Id)))
                .ReturnsAsync(publishedAdvert);

            _userIdentifierProviderMock
                .Setup(x => x.IdentityUserId)
                .Returns(currentUserId);

            _userRepositoryMock
                .Setup(x => x.IsOwner(It.Is<UserId>(id => id.Id == publishedAdvert.AuthorId.Id), currentUserId))
                .ReturnsAsync(true);

            _unitOfWorkMock
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();

            _advertRepositoryMock.Verify(
                x => x.Delete(It.Is<Advert>(a => a == publishedAdvert)),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Fail_When_Advert_Not_Found()
        {
            // Arrange
            var command = new DeletePublishedAdvertCommand(Guid.NewGuid());

            _advertRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<AdvertId>()))
                .ReturnsAsync(Maybe<Advert>.None);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AdvertErrors.NotFound);

            _advertRepositoryMock.Verify(
                x => x.Delete(It.IsAny<Advert>()),
                Times.Never);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Fail_When_User_Not_Owner()
        {
            // Arrange
            var command = new DeletePublishedAdvertCommand(Guid.NewGuid());
            var publishedAdvert = CreateTestPublishedAdvert();
            var differentUserId = "auth0|different-user";

            _advertRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<AdvertId>()))
                .ReturnsAsync(publishedAdvert);

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

            _advertRepositoryMock.Verify(
                x => x.Delete(It.IsAny<Advert>()),
                Times.Never);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Use_Correct_Advert_Id()
        {
            // Arrange
            var expectedAdvertId = Guid.NewGuid();
            var command = new DeletePublishedAdvertCommand(expectedAdvertId);
            var publishedAdvert = CreateTestPublishedAdvert();
            var currentUserId = "auth0|current-user";

            _advertRepositoryMock
                .Setup(x => x.GetByIdAsync(It.Is<AdvertId>(id => id.Id == expectedAdvertId)))
                .ReturnsAsync(publishedAdvert);

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
            var command = new DeletePublishedAdvertCommand(Guid.NewGuid());
            var publishedAdvert = CreateTestPublishedAdvert();
            var expectedCurrentUserId = "auth0|specific-user";

            _advertRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<AdvertId>()))
                .ReturnsAsync(publishedAdvert);

            _userIdentifierProviderMock
                .Setup(x => x.IdentityUserId)
                .Returns(expectedCurrentUserId);

            _userRepositoryMock
                .Setup(x => x.IsOwner(It.Is<UserId>(id => id.Id == publishedAdvert.AuthorId.Id), expectedCurrentUserId))
                .ReturnsAsync(true);

            _unitOfWorkMock
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();

            _userRepositoryMock.Verify(
                x => x.IsOwner(It.Is<UserId>(id => id.Id == publishedAdvert.AuthorId.Id), expectedCurrentUserId),
                Times.Once);
        }

        private static Advert CreateTestPublishedAdvert()
        {
            var authorId = new UserId(Guid.NewGuid());
            var categoryId = new CategoryId(Guid.NewGuid());
            var title = AdvertTitle.Create("Test Title").Value;
            var description = AdvertDescription.Create("Test Description").Value;
            var price = Price.Create(100m).Value;

            var advert = Advert.CreateDraft(authorId, categoryId, title, description, price);

            var imageResult = advert.AddImage(
             ImageFileName.Create("test.jpg").Value,
             ImageContentType.Create("image/jpeg").Value,
             ImageContent.Create(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0x00 }).Value);
            advert.Publish(); 

            advert.ClearDomainEvents(); 

            return advert;
        }
    }
}
