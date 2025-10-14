using FluentAssertions;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.Domain.Users;

namespace SolarLab.AdvertBoard.UnitTests.Domain.Users
{
    public class MiddleNameTests
    {
        [Theory]
        [InlineData("Michael")]
        [InlineData("James")]
        [InlineData("Anne-Marie")]
        [InlineData("Van")]
        [InlineData("Петрович")]
        [InlineData("José")]
        [InlineData("François")]
        public void Create_Should_Succeed_When_MiddleName_Valid(string validMiddleName)
        {
            // Act
            var result = MiddleName.Create(validMiddleName);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(validMiddleName);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Create_Should_Succeed_When_MiddleName_Empty(string emptyMiddleName)
        {
            // Act
            var result = MiddleName.Create(emptyMiddleName);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeNull();
        }

        [Fact]
        public void Create_Should_Fail_When_MiddleName_Too_Long()
        {
            // Arrange
            var longMiddleName = new string('a', MiddleName.MaxLength + 1);

            // Act
            var result = MiddleName.Create(longMiddleName);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.MiddleName.TooLong);
        }

        [Theory]
        [InlineData("Michael123")]
        [InlineData("James@Doe")]
        [InlineData("John_Doe")]
        [InlineData("Brown!")]
        [InlineData("Williams.")]
        [InlineData(" Davis")]
        [InlineData("Miller ")]
        [InlineData("Wilson  Smith")]
        [InlineData("123Johnson")]
        public void Create_Should_Fail_When_MiddleName_Invalid_Format(string invalidMiddleName)
        {
            // Act
            var result = MiddleName.Create(invalidMiddleName);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.MiddleName.NotValid);
        }

        [Fact]
        public void Create_Should_Fail_With_Correct_Error_For_Each_Validation()
        {
            // Test too long
            var longMiddleName = new string('a', MiddleName.MaxLength + 1);
            var longResult = MiddleName.Create(longMiddleName);
            longResult.IsFailure.Should().BeTrue();
            longResult.Error.Should().Be(UserErrors.MiddleName.TooLong);

            // Test invalid format
            var invalidResult = MiddleName.Create("Michael123");
            invalidResult.IsFailure.Should().BeTrue();
            invalidResult.Error.Should().Be(UserErrors.MiddleName.NotValid);
        }


        [Fact]
        public void Create_Should_Handle_Max_Length_MiddleName()
        {
            // Arrange
            var maxLengthMiddleName = new string('a', MiddleName.MaxLength);

            // Act
            var result = MiddleName.Create(maxLengthMiddleName);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(maxLengthMiddleName);
        }

        [Theory]
        [InlineData("De")]
        [InlineData("Van")]
        [InlineData("Di")]
        [InlineData("O'")]
        [InlineData("Mac")]
        public void Create_Should_Succeed_When_MiddleName_With_Special_Characters(string validMiddleName)
        {
            // Act
            var result = MiddleName.Create(validMiddleName);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(validMiddleName);
        }

        [Fact]
        public void Create_Should_Succeed_When_MiddleName_With_Unicode_Characters()
        {
            // Arrange
            var unicodeMiddleName = "Петрович";

            // Act
            var result = MiddleName.Create(unicodeMiddleName);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(unicodeMiddleName);
        }

        [Fact]
        public void Create_Should_Use_Same_Regex_As_FirstName()
        {
            // Arrange
            var validName = "Michael";

            // Act
            var firstNameResult = FirstName.Create(validName);
            var middleNameResult = MiddleName.Create(validName);

            // Assert
            firstNameResult.IsSuccess.Should().BeTrue();
            middleNameResult.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void Create_Should_Return_Null_For_Empty_String()
        {
            // Act
            var result = MiddleName.Create("");

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeNull();
        }

        [Fact]
        public void Create_Should_Return_Null_For_Whitespace_String()
        {
            // Act
            var result = MiddleName.Create("   ");

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeNull();
        }

        [Fact]
        public void Create_Should_Return_Null_For_Null_Input()
        {
            // Act
            var result = MiddleName.Create(null);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeNull();
        }
    }
}
