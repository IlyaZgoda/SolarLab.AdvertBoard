using AutoFixture;
using FluentAssertions;
using Moq;
using SolarLab.AdvertBoard.Application.Abstractions;
using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Comments.Create;
using SolarLab.AdvertBoard.Application.Users.Specifications;
using SolarLab.AdvertBoard.Contracts.Comments;
using SolarLab.AdvertBoard.Domain.AdvertImages;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Categories;
using SolarLab.AdvertBoard.Domain.Comments;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel.Maybe;

namespace SolarLab.AdvertBoard.UnitTests.Application.Comments.Create
{
    public class CreateCommentCommandHandlerTests
    {
        private readonly Mock<IUserIdentifierProvider> _userIdentifierProviderMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IAdvertRepository> _advertRepositoryMock;
        private readonly Mock<ICommentRepository> _commentRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly CreateCommentCommandHandler _handler;

        public CreateCommentCommandHandlerTests()
        {
            _userIdentifierProviderMock = new Mock<IUserIdentifierProvider>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _advertRepositoryMock = new Mock<IAdvertRepository>();
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _handler = new CreateCommentCommandHandler(
                _userIdentifierProviderMock.Object,
                _userRepositoryMock.Object,
                _advertRepositoryMock.Object,
                _commentRepositoryMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Create_Comment_When_All_Data_Valid()
        {
            // Arrange
            var advertId = Guid.NewGuid();
            var commentText = "This is a valid comment text";
            var command = new CreateCommentCommand(advertId, commentText);

            var identityId = "auth0|123456789";
            var user = CreateTestUser(identityId);
            var advert = CreateTestAdvert(user.Id);

            var imageResult = advert.AddImage(
            ImageFileName.Create("test.jpg").Value,
            ImageContentType.Create("image/jpeg").Value,
            ImageContent.Create(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0x00 }).Value);
            advert.Publish();

            _userIdentifierProviderMock
                .Setup(x => x.IdentityUserId)
                .Returns(identityId);

            _userRepositoryMock
                .Setup(x => x.GetBySpecificationAsync(It.IsAny<UserWithIdentitySpecification>()))
                .ReturnsAsync(user);

            _advertRepositoryMock
                .Setup(x => x.GetByIdAsync(It.Is<AdvertId>(id => id.Id == advertId)))
                .ReturnsAsync(advert);

            _unitOfWorkMock
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeOfType<CommentIdResponse>();

            _commentRepositoryMock.Verify(
                x => x.Add(It.Is<Comment>(c =>
                    c.AdvertId == advert.Id &&
                    c.AuthorId == user.Id &&
                    c.Text.Value == commentText &&
                    c.CreatedAt != default)),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }


        [Fact]
        public async Task Handle_Should_Fail_When_Advert_Not_Found()
        {
            // Arrange
            var advertId = Guid.NewGuid();
            var commentText = "This is a valid comment text";
            var command = new CreateCommentCommand(advertId, commentText);

            var identityId = "auth0|123456789";
            var user = CreateTestUser(identityId);

            _userIdentifierProviderMock
                .Setup(x => x.IdentityUserId)
                .Returns(identityId);

            _userRepositoryMock
                .Setup(x => x.GetBySpecificationAsync(It.IsAny<UserWithIdentitySpecification>()))
                .ReturnsAsync(user);

            _advertRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<AdvertId>()))
                .ReturnsAsync(Maybe<Advert>.None);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AdvertErrors.NotFound);

            _commentRepositoryMock.Verify(
                x => x.Add(It.IsAny<Comment>()),
                Times.Never);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Fail_When_Comment_Text_Empty()
        {
            // Arrange
            var advertId = Guid.NewGuid();
            var commentText = "";
            var command = new CreateCommentCommand(advertId, commentText);

            var identityId = "auth0|123456789";
            var user = CreateTestUser(identityId);
            var advert = CreateTestAdvert(user.Id);

            var imageResult = advert.AddImage(
            ImageFileName.Create("test.jpg").Value,
            ImageContentType.Create("image/jpeg").Value,
            ImageContent.Create(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0x00 }).Value);
            advert.Publish();

            _userIdentifierProviderMock
                .Setup(x => x.IdentityUserId)
                .Returns(identityId);

            _userRepositoryMock
                .Setup(x => x.GetBySpecificationAsync(It.IsAny<UserWithIdentitySpecification>()))
                .ReturnsAsync(user);

            _advertRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<AdvertId>()))
                .ReturnsAsync(advert);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CommentErrors.Text.Empty);

            _commentRepositoryMock.Verify(
                x => x.Add(It.IsAny<Comment>()),
                Times.Never);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Return_CommentId_Response_With_Correct_Id()
        {
            // Arrange
            var advertId = Guid.NewGuid();
            var commentText = "This is a valid comment text";
            var command = new CreateCommentCommand(advertId, commentText);

            var identityId = "auth0|123456789";
            var user = CreateTestUser(identityId);
            var advert = CreateTestAdvert(user.Id);

            var imageResult = advert.AddImage(
            ImageFileName.Create("test.jpg").Value,
            ImageContentType.Create("image/jpeg").Value,
            ImageContent.Create(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0x00 }).Value);
            advert.Publish();

            _userIdentifierProviderMock
                .Setup(x => x.IdentityUserId)
                .Returns(identityId);

            _userRepositoryMock
                .Setup(x => x.GetBySpecificationAsync(It.IsAny<UserWithIdentitySpecification>()))
                .ReturnsAsync(user);

            _advertRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<AdvertId>()))
                .ReturnsAsync(advert);

            Comment capturedComment = null;
            _commentRepositoryMock
                .Setup(x => x.Add(It.IsAny<Comment>()))
                .Callback<Comment>(c => capturedComment = c);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeOfType<CommentIdResponse>();
            result.Value.Id.Should().Be(capturedComment.Id);
        }

        private static User CreateTestUser(string identityId)
        {
            var firstName = FirstName.Create("John").Value;
            var lastName = LastName.Create("Doe").Value;
            var contactEmail = ContactEmail.Create("john.doe@example.com").Value;

            return User.Create(identityId, firstName, lastName, null, contactEmail, null);
        }

        private static Advert CreateTestAdvert(Guid userId)
        {
            var title = AdvertTitle.Create("Test Advert").Value;
            var description = AdvertDescription.Create("Test Description").Value;
            var price = Price.Create(1000).Value;
            var categoryId = new CategoryId(Guid.NewGuid());

            return Advert.CreateDraft(
                new UserId(userId),
                categoryId,
                title,
                description,
                price);
        }
    }
}
