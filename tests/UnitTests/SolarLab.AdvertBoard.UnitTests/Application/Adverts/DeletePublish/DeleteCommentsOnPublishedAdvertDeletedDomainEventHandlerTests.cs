using FluentAssertions;
using Moq;
using SolarLab.AdvertBoard.Application.Abstractions;
using SolarLab.AdvertBoard.Application.Adverts.DeletePublished;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Adverts.Events;
using SolarLab.AdvertBoard.Domain.Comments;

namespace SolarLab.AdvertBoard.UnitTests.Application.Adverts.DeletePublish
{
    public class DeleteCommentsOnPublishedAdvertDeletedDomainEventHandlerTests
    {
        private readonly Mock<ICommentRepository> _commentRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly DeleteCommentsOnPublishedAdvertDeletedDomainEventHandler _handler;

        public DeleteCommentsOnPublishedAdvertDeletedDomainEventHandlerTests()
        {
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new DeleteCommentsOnPublishedAdvertDeletedDomainEventHandler(
                _commentRepositoryMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Delete_All_Comments_For_Advert_When_Event_Received()
        {
            // Arrange
            var advertId = Guid.NewGuid();
            var domainEvent = new PublishedAdvertDeletedDomainEvent(new AdvertId(advertId));

            _commentRepositoryMock
                .Setup(x => x.DeleteAllForAdvert(It.Is<AdvertId>(id => id.Id == advertId)))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            // Act
            await _handler.Handle(domainEvent, CancellationToken.None);

            // Assert
            _commentRepositoryMock.Verify(
                x => x.DeleteAllForAdvert(It.Is<AdvertId>(id => id.Id == advertId)),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
