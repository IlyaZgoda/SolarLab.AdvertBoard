using FluentAssertions;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.Domain.Users;

namespace SolarLab.AdvertBoard.UnitTests.Domain.Users
{
    public class PhoneNumberTests
    {
        [Theory]
        [InlineData("+12345678901")] 
        [InlineData("+123456789012")] 
        [InlineData("+1234567890123")] 
        [InlineData("+12345678901234")] 
        [InlineData("+71234567890")] 
        [InlineData("+441234567890")] 
        public void Create_Should_Succeed_When_PhoneNumber_Valid(string validPhoneNumber)
        {
            // Act
            var result = PhoneNumber.Create(validPhoneNumber);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(validPhoneNumber);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Create_Should_Succeed_When_PhoneNumber_Empty(string emptyPhoneNumber)
        {
            // Act
            var result = PhoneNumber.Create(emptyPhoneNumber);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeNull();
        }

        [Fact]
        public void Create_Should_Fail_When_PhoneNumber_Too_Long()
        {
            // Arrange
            var longPhoneNumber = "+123456789012345"; 

            // Act
            var result = PhoneNumber.Create(longPhoneNumber);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.PhoneNumber.TooLong);
        }

        [Fact]
        public void Create_Should_Fail_When_PhoneNumber_Too_Short()
        {
            // Arrange
            var shortPhoneNumber = "+1234567890"; 

            // Act
            var result = PhoneNumber.Create(shortPhoneNumber);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.PhoneNumber.NotValid);
        }

        [Theory]
        [InlineData("12345678901")] 
        [InlineData("+1234567890")] 
        [InlineData("+abc12345678")] 
        [InlineData("+12-345-6789")] 
        [InlineData("+12 345 6789")] 
        [InlineData("+1(234)567890")] 
        [InlineData("01234567890")] 
        [InlineData("+")] 
        [InlineData("+123")] 
        public void Create_Should_Fail_When_PhoneNumber_Invalid_Format(string invalidPhoneNumber)
        {
            // Act
            var result = PhoneNumber.Create(invalidPhoneNumber);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.PhoneNumber.NotValid);
        }

        [Fact]
        public void Create_Should_Fail_With_Correct_Error_For_Each_Validation()
        {
            // Test too long
            var longPhoneNumber = "+123456789012345";
            var longResult = PhoneNumber.Create(longPhoneNumber);
            longResult.IsFailure.Should().BeTrue();
            longResult.Error.Should().Be(UserErrors.PhoneNumber.TooLong);

            // Test invalid format
            var invalidResult = PhoneNumber.Create("12345678901");
            invalidResult.IsFailure.Should().BeTrue();
            invalidResult.Error.Should().Be(UserErrors.PhoneNumber.NotValid);
        }

        [Fact]
        public void Create_Should_Handle_Max_Length_PhoneNumber()
        {
            // Arrange
            var maxLengthPhoneNumber = "+12345678901234"; 

            // Act
            var result = PhoneNumber.Create(maxLengthPhoneNumber);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(maxLengthPhoneNumber);
        }

        [Fact]
        public void Create_Should_Handle_Min_Length_PhoneNumber()
        {
            // Arrange
            var minLengthPhoneNumber = "+12345678901"; 

            // Act
            var result = PhoneNumber.Create(minLengthPhoneNumber);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(minLengthPhoneNumber);
        }

        [Fact]
        public void Create_Should_Return_Null_For_Empty_String()
        {
            // Act
            var result = PhoneNumber.Create("");

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeNull();
        }

        [Fact]
        public void Create_Should_Return_Null_For_Whitespace_String()
        {
            // Act
            var result = PhoneNumber.Create("   ");

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeNull();
        }

        [Fact]
        public void Create_Should_Return_Null_For_Null_Input()
        {
            // Act
            var result = PhoneNumber.Create(null);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeNull();
        }

        [Fact]
        public void Create_Should_Accept_International_Formats()
        {
            // Test various international formats
            var usNumber = "+12025550123"; 
            var ukNumber = "+441632960123"; 
            var deNumber = "+49301234567"; 
            var frNumber = "+33123456789"; 

            PhoneNumber.Create(usNumber).IsSuccess.Should().BeTrue();
            PhoneNumber.Create(ukNumber).IsSuccess.Should().BeTrue();
            PhoneNumber.Create(deNumber).IsSuccess.Should().BeTrue();
            PhoneNumber.Create(frNumber).IsSuccess.Should().BeTrue();
        }
    }
}
