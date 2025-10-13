using AutoFixture;
using FluentAssertions;
using FluentValidation.TestHelper;
using SolarLab.AdvertBoard.Application.Comments.Create;
using SolarLab.AdvertBoard.Contracts.Comments;
using SolarLab.AdvertBoard.Domain.Comments;
using SolarLab.AdvertBoard.Domain.Errors;

namespace SolarLab.AdvertBoard.UnitTests.Application.Validators
{
    public class CreateCommentRequestValidatorTests
    {
        private readonly CreateCommentRequestValidator _validator;
        private readonly Fixture _fixture;

        public CreateCommentRequestValidatorTests()
        {
            _validator = new CreateCommentRequestValidator();
            _fixture = new Fixture();
        }

        [Fact]
        public void Should_Not_Have_Errors_When_Valid()
        {
            // Arrange
            var request = _fixture
                .Build<CreateCommentRequest>()
                .With(x => x.Text, "This is a valid comment text for testing purposes.")
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Should_Have_Error_When_Text_Is_Empty(string emptyText)
        {
            // Arrange
            var request = _fixture
                .Build<CreateCommentRequest>()
                .With(x => x.Text, emptyText)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Text)
                  .WithErrorMessage(CommentErrors.Text.Empty.Description);
        }

        [Fact]
        public void Should_Have_Error_When_Text_Too_Long()
        {
            // Arrange
            var longText = new string('a', CommentText.MaxLength + 1);
            var request = _fixture
                .Build<CreateCommentRequest>()
                .With(x => x.Text, longText)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Text)
                  .WithErrorMessage(CommentErrors.Text.TooLong.Description);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(CommentText.MaxLength - 1)]
        [InlineData(CommentText.MaxLength)]
        public void Should_Not_Have_Error_When_Text_Length_Valid(int textLength)
        {
            // Arrange
            var validText = new string('a', textLength);
            var request = _fixture
                .Build<CreateCommentRequest>()
                .With(x => x.Text, validText)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Text);
        }

        [Fact]
        public void Should_Pass_When_Text_Is_Minimum_Length()
        {
            // Arrange
            var minimalText = "a"; 
            var request = _fixture
                .Build<CreateCommentRequest>()
                .With(x => x.Text, minimalText)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Text);
        }

        [Fact]
        public void Should_Pass_With_Realistic_Comment_Scenario()
        {
            // Arrange
            var request = _fixture
                .Build<CreateCommentRequest>()
                .With(x => x.Text, "This product is amazing! I really love the quality and the fast delivery. Highly recommended!")
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_Pass_With_Special_Characters_In_Text()
        {
            // Arrange
            var request = _fixture
                .Build<CreateCommentRequest>()
                .With(x => x.Text, "Great product! 👍😊 I love it! #best #quality @mention")
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Text);
        }

        [Fact]
        public void Should_Have_Only_One_Error_When_Text_Empty()
        {
            // Arrange
            var request = _fixture
                .Build<CreateCommentRequest>()
                .With(x => x.Text, "")
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.Errors.Should().ContainSingle();
            result.ShouldHaveValidationErrorFor(x => x.Text)
                  .WithErrorMessage(CommentErrors.Text.Empty.Description);
        }

        [Fact]
        public void Should_Have_Only_One_Error_When_Text_Too_Long()
        {
            // Arrange
            var longText = new string('a', CommentText.MaxLength + 1);
            var request = _fixture
                .Build<CreateCommentRequest>()
                .With(x => x.Text, longText)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.Errors.Should().ContainSingle();
            result.ShouldHaveValidationErrorFor(x => x.Text)
                  .WithErrorMessage(CommentErrors.Text.TooLong.Description);
        }
    }
}
