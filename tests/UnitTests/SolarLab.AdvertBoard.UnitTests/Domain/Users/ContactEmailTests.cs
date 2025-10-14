using FluentAssertions;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.Domain.Users;

namespace SolarLab.AdvertBoard.UnitTests.Domain.Users
{
    public class ContactEmailTests
    {
        [Theory]
        [InlineData("test@example.com")]
        [InlineData("user.name@domain.co.uk")]
        [InlineData("test+filter@example.com")]
        [InlineData("user@sub.domain.com")]
        public void Create_Should_Succeed_When_Email_Valid(string validEmail)
        {
            // Act
            var result = ContactEmail.Create(validEmail);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(validEmail);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Create_Should_Fail_When_Email_Empty(string emptyEmail)
        {
            // Act
            var result = ContactEmail.Create(emptyEmail);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.Email.Empty);
        }

        [Fact]
        public void Create_Should_Fail_When_Email_Too_Long()
        {
            // Arrange
            var longEmail = new string('a', 310) + "@example.com";

            // Act
            var result = ContactEmail.Create(longEmail);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.Email.TooLong);
        }

        [Theory]
        [InlineData("invalid-email")]
        [InlineData("invalid.email")]
        [InlineData("@domain.com")]
        [InlineData("test@")]
        [InlineData("test@domain")]
        [InlineData("test@.com")]
        public void Create_Should_Fail_When_Email_Invalid_Format(string invalidEmail)
        {
            // Act
            var result = ContactEmail.Create(invalidEmail);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.Email.NotValid);
        }

        [Fact]
        public void Create_Should_Fail_With_Correct_Error_For_Each_Validation()
        {
            // Test empty
            var emptyResult = ContactEmail.Create("");
            emptyResult.IsFailure.Should().BeTrue();
            emptyResult.Error.Should().Be(UserErrors.Email.Empty);

            // Test too long
            var longEmail = new string('a', 321) + "@test.com";
            var longResult = ContactEmail.Create(longEmail);
            longResult.IsFailure.Should().BeTrue();
            longResult.Error.Should().Be(UserErrors.Email.TooLong);

            // Test invalid format
            var invalidResult = ContactEmail.Create("invalid");
            invalidResult.IsFailure.Should().BeTrue();
            invalidResult.Error.Should().Be(UserErrors.Email.NotValid);
        }

        [Fact]
        public void Explicit_Operator_Should_Return_String_Value()
        {
            // Arrange
            var email = "test@example.com";
            var contactEmail = ContactEmail.Create(email).Value;

            // Act
            string result = (string)contactEmail;

            // Assert
            result.Should().Be(email);
        }


        [Fact]
        public void Create_Should_Handle_Max_Length_Email()
        {
            // Arrange
            var maxLengthEmail = new string('a', 310) + "@b.com";

            // Act
            var result = ContactEmail.Create(maxLengthEmail);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(maxLengthEmail);
        }
    }
}
