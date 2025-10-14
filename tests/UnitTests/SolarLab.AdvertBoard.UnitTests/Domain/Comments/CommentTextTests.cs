using FluentAssertions;
using SolarLab.AdvertBoard.Domain.Comments;
using SolarLab.AdvertBoard.Domain.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarLab.AdvertBoard.UnitTests.Domain.Comments
{
    public class CommentTextTests
    {
        [Theory]
        [InlineData("This is a valid comment")]
        [InlineData("Great product!")]
        [InlineData("I really like this item")]
        [InlineData("Fast delivery, good quality")]
        public void Create_Should_Succeed_When_Text_Valid(string validText)
        {
            // Act
            var result = CommentText.Create(validText);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(validText);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Create_Should_Fail_When_Text_Empty(string emptyText)
        {
            // Act
            var result = CommentText.Create(emptyText);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CommentErrors.Text.Empty);
        }

        [Fact]
        public void Create_Should_Fail_When_Text_Too_Long()
        {
            // Arrange
            var longText = new string('a', CommentText.MaxLength + 1);

            // Act
            var result = CommentText.Create(longText);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CommentErrors.Text.TooLong);
        }

        [Fact]
        public void Create_Should_Fail_With_Correct_Error_For_Each_Validation()
        {
            // Test empty
            var emptyResult = CommentText.Create("");
            emptyResult.IsFailure.Should().BeTrue();
            emptyResult.Error.Should().Be(CommentErrors.Text.Empty);

            // Test too long
            var longText = new string('a', CommentText.MaxLength + 1);
            var longResult = CommentText.Create(longText);
            longResult.IsFailure.Should().BeTrue();
            longResult.Error.Should().Be(CommentErrors.Text.TooLong);
        }

        [Fact]
        public void Create_Should_Handle_Max_Length_Text()
        {
            // Arrange
            var maxLengthText = new string('a', CommentText.MaxLength);

            // Act
            var result = CommentText.Create(maxLengthText);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(maxLengthText);
        }

        [Fact]
        public void Create_Should_Handle_Min_Length_Text()
        {
            // Arrange
            var minLengthText = "a"; // 1 character

            // Act
            var result = CommentText.Create(minLengthText);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(minLengthText);
        }

        [Theory]
        [InlineData("👍")]
        [InlineData("Great! 😊")]
        [InlineData("I love it! ❤️")]
        [InlineData("Fast delivery 🚚")]
        public void Create_Should_Succeed_When_Text_With_Emojis(string textWithEmojis)
        {
            // Act
            var result = CommentText.Create(textWithEmojis);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(textWithEmojis);
        }

        [Fact]
        public void Create_Should_Succeed_When_Text_With_Special_Characters()
        {
            // Arrange
            var textWithSymbols = "Great product! Price: $100. Contact: example@test.com #best";

            // Act
            var result = CommentText.Create(textWithSymbols);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(textWithSymbols);
        }

        [Fact]
        public void Create_Should_Succeed_When_Text_With_Unicode_Characters()
        {
            // Arrange
            var unicodeText = "Отличный товар! Рекомендую. Цена хорошая.";

            // Act
            var result = CommentText.Create(unicodeText);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(unicodeText);
        }

        [Fact]
        public void Create_Should_Succeed_When_Text_With_NewLines()
        {
            // Arrange
            var multilineText = "First line of comment.\nSecond line.\nThird line.";

            // Act
            var result = CommentText.Create(multilineText);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(multilineText);
        }

        [Fact]
        public void Create_Should_Succeed_When_Text_With_Urls()
        {
            // Arrange
            var textWithUrls = "Check this out: https://example.com and http://test.org";

            // Act
            var result = CommentText.Create(textWithUrls);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(textWithUrls);
        }

        [Fact]
        public void Create_Should_Succeed_With_Realistic_Long_Comment()
        {
            // Arrange
            var longComment = @"I purchased this product last week and I must say I'm very impressed with the quality. 
                The build is solid, the materials feel premium, and it performs exactly as advertised. 
                The delivery was faster than expected and the packaging was secure. 
                I would definitely recommend this to others looking for a reliable solution. 
                Five stars! ⭐⭐⭐⭐⭐";

            // Act
            var result = CommentText.Create(longComment);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(longComment);
        }
    }
}
