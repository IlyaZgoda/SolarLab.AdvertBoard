using FluentAssertions;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.Domain.Users;

namespace SolarLab.AdvertBoard.UnitTests.Domain.Users
{
    public class LastNameTests
    {
        [Theory]
        [InlineData("Doe")]
        [InlineData("Smith")]
        [InlineData("O'Brien")]
        [InlineData("Van der Berg")]
        [InlineData("Иванов")]
        [InlineData("García")]
        [InlineData("Müller")]
        public void Create_Should_Succeed_When_LastName_Valid(string validLastName)
        {
            // Act
            var result = LastName.Create(validLastName);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(validLastName);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Create_Should_Fail_When_LastName_Empty(string emptyLastName)
        {
            // Act
            var result = LastName.Create(emptyLastName);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.LastName.Empty);
        }

        [Fact]
        public void Create_Should_Fail_When_LastName_Too_Long()
        {
            // Arrange
            var longLastName = new string('a', LastName.MaxLength + 1);

            // Act
            var result = LastName.Create(longLastName);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.LastName.TooLong);
        }

        [Theory]
        [InlineData("Doe123")]
        [InlineData("Smith@Jones")]
        [InlineData("Johnson_Doe")]
        [InlineData("Brown!")]
        [InlineData("Williams.")]
        [InlineData(" Davis")]
        [InlineData("Miller ")]
        [InlineData("Wilson  Smith")]
        [InlineData("123Johnson")]
        public void Create_Should_Fail_When_LastName_Invalid_Format(string invalidLastName)
        {
            // Act
            var result = LastName.Create(invalidLastName);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.LastName.NotValid);
        }

        [Fact]
        public void Create_Should_Fail_With_Correct_Error_For_Each_Validation()
        {
            // Test empty
            var emptyResult = LastName.Create("");
            emptyResult.IsFailure.Should().BeTrue();
            emptyResult.Error.Should().Be(UserErrors.LastName.Empty);

            // Test too long
            var longLastName = new string('a', LastName.MaxLength + 1);
            var longResult = LastName.Create(longLastName);
            longResult.IsFailure.Should().BeTrue();
            longResult.Error.Should().Be(UserErrors.LastName.TooLong);

            // Test invalid format
            var invalidResult = LastName.Create("Doe123");
            invalidResult.IsFailure.Should().BeTrue();
            invalidResult.Error.Should().Be(UserErrors.LastName.NotValid);
        }

        [Fact]
        public void Create_Should_Handle_Max_Length_LastName()
        {
            // Arrange
            var maxLengthLastName = new string('a', LastName.MaxLength);

            // Act
            var result = LastName.Create(maxLengthLastName);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(maxLengthLastName);
        }

        [Theory]
        [InlineData("Van der Berg")]
        [InlineData("De la Cruz")]
        [InlineData("Di Marco")]
        [InlineData("O'Reilly")]
        [InlineData("MacDonald")]
        public void Create_Should_Succeed_When_LastName_With_Spaces_And_Special_Characters(string validLastName)
        {
            // Act
            var result = LastName.Create(validLastName);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(validLastName);
        }

        [Fact]
        public void Create_Should_Succeed_When_LastName_With_Unicode_Characters()
        {
            // Arrange
            var unicodeLastName = "Иванов-Петров";

            // Act
            var result = LastName.Create(unicodeLastName);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(unicodeLastName);
        }

        [Fact]
        public void Create_Should_Use_Same_Regex_As_FirstName()
        {
            // Arrange
            var validName = "Smith";

            // Act
            var firstNameResult = FirstName.Create(validName);
            var lastNameResult = LastName.Create(validName);

            // Assert
            firstNameResult.IsSuccess.Should().BeTrue();
            lastNameResult.IsSuccess.Should().BeTrue();
        }
    }
}
