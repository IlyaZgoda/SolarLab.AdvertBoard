using FluentAssertions;
using Moq;
using SolarLab.AdvertBoard.Application.Abstractions;
using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Comments.Update;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Comments;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel.Maybe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarLab.AdvertBoard.UnitTests.Application.Comments.Update
{
    public class UpdateCommentCommandHandlerTests
    {
        private readonly Mock<IUserIdentifierProvider> _userIdentifierProviderMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ICommentRepository> _commentRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly UpdateCommentCommandHandler _handler;

        public UpdateCommentCommandHandlerTests()
        {
            _userIdentifierProviderMock = new Mock<IUserIdentifierProvider>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new UpdateCommentCommandHandler(
                _userIdentifierProviderMock.Object,
                _userRepositoryMock.Object,
                _commentRepositoryMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Update_Comment_When_All_Data_Valid()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var updatedText = "Updated comment text";
            var command = new UpdateCommentCommand(commentId, updatedText);

            var comment = CreateTestComment();
            var currentUserId = "auth0|current-user";

            _commentRepositoryMock
                .Setup(x => x.GetByIdAsync(It.Is<CommentId>(id => id.Id == commentId)))
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
                x => x.Update(It.Is<Comment>(c =>
                    c.Id == comment.Id &&
                    c.Text.Value == updatedText &&
                    c.UpdatedAt != null)),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Fail_When_Comment_Not_Found()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var updatedText = "Updated comment text";
            var command = new UpdateCommentCommand(commentId, updatedText);

            _commentRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<CommentId>()))
                .ReturnsAsync(Maybe<Comment>.None);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CommentErrors.NotFound);

            _commentRepositoryMock.Verify(
                x => x.Update(It.IsAny<Comment>()),
                Times.Never);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Fail_When_User_Not_Owner()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var updatedText = "Updated comment text";
            var command = new UpdateCommentCommand(commentId, updatedText);

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
                x => x.Update(It.IsAny<Comment>()),
                Times.Never);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Fail_When_Comment_Text_Empty()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var updatedText = "";
            var command = new UpdateCommentCommand(commentId, updatedText);

            var comment = CreateTestComment();
            var currentUserId = "auth0|current-user";

            _commentRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<CommentId>()))
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
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CommentErrors.Text.Empty);

            _commentRepositoryMock.Verify(
                x => x.Update(It.IsAny<Comment>()),
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
            var updatedText = "Updated comment text";
            var command = new UpdateCommentCommand(expectedCommentId, updatedText);

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

            _unitOfWorkMock
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

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
            var commentId = Guid.NewGuid();
            var updatedText = "Updated comment text";
            var command = new UpdateCommentCommand(commentId, updatedText);

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

            _unitOfWorkMock
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();

            _userRepositoryMock.Verify(
                x => x.IsOwner(It.Is<UserId>(id => id.Id == comment.AuthorId.Id), expectedCurrentUserId),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Set_UpdatedAt_When_Comment_Updated()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var updatedText = "Updated comment text";
            var command = new UpdateCommentCommand(commentId, updatedText);

            var comment = CreateTestComment();
            var currentUserId = "auth0|current-user";

            _commentRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<CommentId>()))
                .ReturnsAsync(comment);

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

            _commentRepositoryMock.Verify(
                x => x.Update(It.Is<Comment>(c => c.UpdatedAt != null)),
                Times.Once);
        }
        private static Comment CreateTestComment()
        {
            var advertId = new AdvertId(Guid.NewGuid());
            var authorId = new UserId(Guid.NewGuid());
            var text = CommentText.Create("Original comment text").Value;

            return Comment.Create(advertId, authorId, text);
        }
    }
}
