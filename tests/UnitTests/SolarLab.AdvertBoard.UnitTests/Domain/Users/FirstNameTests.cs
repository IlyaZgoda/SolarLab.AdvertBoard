using FluentAssertions;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.Domain.Users;

namespace SolarLab.AdvertBoard.UnitTests.Domain.Users
{
    public class FirstNameTests
    {
        [Theory]
        [InlineData("John")]
        [InlineData("Mary")]
        [InlineData("Jean-Claude")]
        [InlineData("O'Neill")]
        [InlineData("Анна")]
        [InlineData("José")]
        [InlineData("François")]
        public void Create_Should_Succeed_When_FirstName_Valid(string validFirstName)
        {
            // Act
            var result = FirstName.Create(validFirstName);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(validFirstName);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Create_Should_Fail_When_FirstName_Empty(string emptyFirstName)
        {
            // Act
            var result = FirstName.Create(emptyFirstName);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.FirstName.Empty);
        }

        [Fact]
        public void Create_Should_Fail_When_FirstName_Too_Long()
        {
            // Arrange
            var longFirstName = new string('a', FirstName.MaxLength + 1);

            // Act
            var result = FirstName.Create(longFirstName);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.FirstName.TooLong);
        }

        [Theory]
        [InlineData("John123")]
        [InlineData("John@Doe")]
        [InlineData("John_Doe")]
        [InlineData("John!")]
        [InlineData("John.")]
        [InlineData(" John")]
        [InlineData("John ")]
        [InlineData("John  Doe")]
        [InlineData("123John")]
        public void Create_Should_Fail_When_FirstName_Invalid_Format(string invalidFirstName)
        {
            // Act
            var result = FirstName.Create(invalidFirstName);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.FirstName.NotValid);
        }

        [Fact]
        public void Create_Should_Fail_With_Correct_Error_For_Each_Validation()
        {
            // Test empty
            var emptyResult = FirstName.Create("");
            emptyResult.IsFailure.Should().BeTrue();
            emptyResult.Error.Should().Be(UserErrors.FirstName.Empty);

            // Test too long
            var longFirstName = new string('a', FirstName.MaxLength + 1);
            var longResult = FirstName.Create(longFirstName);
            longResult.IsFailure.Should().BeTrue();
            longResult.Error.Should().Be(UserErrors.FirstName.TooLong);

            // Test invalid format
            var invalidResult = FirstName.Create("John123");
            invalidResult.IsFailure.Should().BeTrue();
            invalidResult.Error.Should().Be(UserErrors.FirstName.NotValid);
        }

        [Fact]
        public void Create_Should_Handle_Max_Length_FirstName()
        {
            // Arrange
            var maxLengthFirstName = new string('a', FirstName.MaxLength);

            // Act
            var result = FirstName.Create(maxLengthFirstName);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(maxLengthFirstName);
        }

        [Theory]
        [InlineData("Anne-Marie")]
        [InlineData("Mary Jane")]
        [InlineData("Jean Pierre")]
        [InlineData("Maria del Carmen")]
        public void Create_Should_Succeed_When_FirstName_With_Spaces_And_Hyphens(string validFirstName)
        {
            // Act
            var result = FirstName.Create(validFirstName);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(validFirstName);
        }

        [Fact]
        public void Create_Should_Succeed_When_FirstName_With_Unicode_Characters()
        {
            // Arrange
            var unicodeFirstName = "Анна-Мария";

            // Act
            var result = FirstName.Create(unicodeFirstName);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(unicodeFirstName);
        }
    }
}
