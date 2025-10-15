using FluentAssertions;
using Moq;
using SolarLab.AdvertBoard.Application.Abstractions;
using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Comments.Delete;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Comments;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel.Maybe;

namespace SolarLab.AdvertBoard.UnitTests.Application.Comments.Delete
{
    public class DeleteCommentCommandHandlerTests
    {
        private readonly Mock<ICommentRepository> _commentRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserIdentifierProvider> _userIdentifierProviderMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly DeleteCommentCommandHandler _handler;

        public DeleteCommentCommandHandlerTests()
        {
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userIdentifierProviderMock = new Mock<IUserIdentifierProvider>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new DeleteCommentCommandHandler(
                _commentRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _userIdentifierProviderMock.Object,
                _userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Delete_Comment_When_User_Is_Owner()
        {
            // Arrange
            var command = new DeleteCommentCommand(Guid.NewGuid());
            var comment = CreateTestComment();
            var currentUserId = "auth0|current-user";

            _commentRepositoryMock
                .Setup(x => x.GetByIdAsync(It.Is<CommentId>(id => id.Id == command.Id)))
                .ReturnsAsync(comment);

            _userIdentifierProviderMock
                .Setup(x => x.IdentityUserId)
                .Returns(currentUserId);

            _userRepositoryMock
                .Setup(x => x.IsOwner(It.Is<UserId>(id => id.Id == comment.AuthorId.Id), currentUserId))
                .ReturnsAsync(true);

            _unitOfWorkMock
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();

            _commentRepositoryMock.Verify(
                x => x.Delete(comment),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Fail_When_Comment_Not_Found()
        {
            // Arrange
            var command = new DeleteCommentCommand(Guid.NewGuid());

            _commentRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<CommentId>()))
                .ReturnsAsync(Maybe<Comment>.None);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CommentErrors.NotFound);

            _commentRepositoryMock.Verify(
                x => x.Delete(It.IsAny<Comment>()),
                Times.Never);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Fail_When_User_Not_Owner()
        {
            // Arrange
            var command = new DeleteCommentCommand(Guid.NewGuid());
            var comment = CreateTestComment();
            var differentUserId = "auth0|different-user";

            _commentRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<CommentId>()))
                .ReturnsAsync(comment);

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
            result.Error.Should().Be(CommentErrors.NotFound);

            _commentRepositoryMock.Verify(
                x => x.Delete(It.IsAny<Comment>()),
                Times.Never);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Use_Correct_Comment_Id()
        {
            // Arrange
            var expectedCommentId = Guid.NewGuid();
            var command = new DeleteCommentCommand(expectedCommentId);
            var comment = CreateTestComment();
            var currentUserId = "auth0|current-user";

            _commentRepositoryMock
                .Setup(x => x.GetByIdAsync(It.Is<CommentId>(id => id.Id == expectedCommentId)))
                .ReturnsAsync(comment);

            _userIdentifierProviderMock
                .Setup(x => x.IdentityUserId)
                .Returns(currentUserId);

            _userRepositoryMock
                .Setup(x => x.IsOwner(It.IsAny<UserId>(), currentUserId))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();

            _commentRepositoryMock.Verify(
                x => x.GetByIdAsync(It.Is<CommentId>(id => id.Id == expectedCommentId)),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Use_Correct_User_For_Ownership_Check()
        {
            // Arrange
            var command = new DeleteCommentCommand(Guid.NewGuid());
            var comment = CreateTestComment();
            var expectedCurrentUserId = "auth0|specific-user";

            _commentRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<CommentId>()))
                .ReturnsAsync(comment);

            _userIdentifierProviderMock
                .Setup(x => x.IdentityUserId)
                .Returns(expectedCurrentUserId);

            _userRepositoryMock
                .Setup(x => x.IsOwner(It.Is<UserId>(id => id.Id == comment.AuthorId.Id), expectedCurrentUserId))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();

            _userRepositoryMock.Verify(
                x => x.IsOwner(It.Is<UserId>(id => id.Id == comment.AuthorId.Id), expectedCurrentUserId),
                Times.Once);
        }

        private static Comment CreateTestComment()
        {
            var advertId = new AdvertId(Guid.NewGuid());
            var authorId = new UserId(Guid.NewGuid());
            var text = CommentText.Create("Test comment text").Value;

            return Comment.Create(advertId, authorId, text);
        }
    }
}
