using FluentAssertions;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarLab.AdvertBoard.UnitTests.Domain.Adverts
{
    public class AdvertTitleTests
    {
        [Theory]
        [InlineData("MacBook Pro 2023")]
        [InlineData("iPhone 15")]
        [InlineData("Car for sale")]
        [InlineData("Apartment rent")]
        [InlineData("New laptop")]
        public void Create_Should_Succeed_When_Title_Valid(string validTitle)
        {
            // Act
            var result = AdvertTitle.Create(validTitle);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(validTitle);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Create_Should_Fail_When_Title_Empty(string emptyTitle)
        {
            // Act
            var result = AdvertTitle.Create(emptyTitle);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AdvertErrors.Title.Empty);
        }

        [Theory]
        [InlineData("ab")] 
        [InlineData("a")]  
        public void Create_Should_Fail_When_Title_Too_Short(string shortTitle)
        {
            // Act
            var result = AdvertTitle.Create(shortTitle);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AdvertErrors.Title.TooShort);
        }

        [Fact]
        public void Create_Should_Fail_When_Title_Too_Long()
        {
            // Arrange
            var longTitle = new string('a', AdvertTitle.MaxLength + 1);

            // Act
            var result = AdvertTitle.Create(longTitle);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AdvertErrors.Title.TooLong);
        }

        [Fact]
        public void Create_Should_Fail_With_Correct_Error_For_Each_Validation()
        {
            // Test empty
            var emptyResult = AdvertTitle.Create("");
            emptyResult.IsFailure.Should().BeTrue();
            emptyResult.Error.Should().Be(AdvertErrors.Title.Empty);

            // Test too short
            var shortResult = AdvertTitle.Create("ab");
            shortResult.IsFailure.Should().BeTrue();
            shortResult.Error.Should().Be(AdvertErrors.Title.TooShort);

            // Test too long
            var longTitle = new string('a', AdvertTitle.MaxLength + 1);
            var longResult = AdvertTitle.Create(longTitle);
            longResult.IsFailure.Should().BeTrue();
            longResult.Error.Should().Be(AdvertErrors.Title.TooLong);
        }

        [Fact]
        public void Create_Should_Handle_Min_Length_Title()
        {
            // Arrange
            var minLengthTitle = new string('a', AdvertTitle.MinLength);

            // Act
            var result = AdvertTitle.Create(minLengthTitle);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(minLengthTitle);
        }

        [Fact]
        public void Create_Should_Handle_Max_Length_Title()
        {
            // Arrange
            var maxLengthTitle = new string('a', AdvertTitle.MaxLength);

            // Act
            var result = AdvertTitle.Create(maxLengthTitle);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(maxLengthTitle);
        }

        [Theory]
        [InlineData("Laptop for sale - excellent condition!")]
        [InlineData("Brand new smartphone with warranty 2024")]
        [InlineData("Beautiful apartment in city center near")]
        public void Create_Should_Succeed_When_Title_With_Special_Characters(string validTitle)
        {
            // Act
            var result = AdvertTitle.Create(validTitle);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(validTitle);
        }

        [Fact]
        public void Create_Should_Succeed_When_Title_With_Numbers_And_Symbols()
        {
            // Arrange
            var titleWithSymbols = "MacBook Pro 16\" M3 Max - 2024 Edition!";

            // Act
            var result = AdvertTitle.Create(titleWithSymbols);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(titleWithSymbols);
        }

        [Fact]
        public void Create_Should_Succeed_When_Title_With_Unicode_Characters()
        {
            // Arrange
            var unicodeTitle = "Ноутбук MacBook Pro 2023";

            // Act
            var result = AdvertTitle.Create(unicodeTitle);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(unicodeTitle);
        }
    }
}
