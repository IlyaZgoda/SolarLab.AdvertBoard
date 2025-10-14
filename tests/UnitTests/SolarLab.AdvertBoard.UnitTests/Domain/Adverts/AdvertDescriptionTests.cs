using FluentAssertions;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Errors;

namespace SolarLab.AdvertBoard.UnitTests.Domain.Adverts
{
    public class AdvertDescriptionTests
    {
        [Theory]
        [InlineData("This is a valid description for testing")]
        [InlineData("MacBook Pro 2023 in excellent condition")]
        [InlineData("Beautiful apartment with great view")]
        [InlineData("Car for sale, low mileage")]
        public void Create_Should_Succeed_When_Description_Valid(string validDescription)
        {
            // Act
            var result = AdvertDescription.Create(validDescription);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(validDescription);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Create_Should_Fail_When_Description_Empty(string emptyDescription)
        {
            // Act
            var result = AdvertDescription.Create(emptyDescription);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AdvertErrors.Description.Empty);
        }

        [Theory]
        [InlineData("abcd")] 
        [InlineData("abc")]  
        [InlineData("a")]     
        public void Create_Should_Fail_When_Description_Too_Short(string shortDescription)
        {
            // Act
            var result = AdvertDescription.Create(shortDescription);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AdvertErrors.Description.TooShort);
        }

        [Fact]
        public void Create_Should_Fail_When_Description_Too_Long()
        {
            // Arrange
            var longDescription = new string('a', AdvertDescription.MaxLength + 1);

            // Act
            var result = AdvertDescription.Create(longDescription);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AdvertErrors.Description.TooLong);
        }

        [Fact]
        public void Create_Should_Fail_With_Correct_Error_For_Each_Validation()
        {
            // Test empty
            var emptyResult = AdvertDescription.Create("");
            emptyResult.IsFailure.Should().BeTrue();
            emptyResult.Error.Should().Be(AdvertErrors.Description.Empty);

            // Test too short
            var shortResult = AdvertDescription.Create("abcd");
            shortResult.IsFailure.Should().BeTrue();
            shortResult.Error.Should().Be(AdvertErrors.Description.TooShort);

            // Test too long
            var longDescription = new string('a', AdvertDescription.MaxLength + 1);
            var longResult = AdvertDescription.Create(longDescription);
            longResult.IsFailure.Should().BeTrue();
            longResult.Error.Should().Be(AdvertErrors.Description.TooLong);
        }

        [Fact]
        public void Create_Should_Handle_Min_Length_Description()
        {
            // Arrange
            var minLengthDescription = new string('a', AdvertDescription.MinLength);

            // Act
            var result = AdvertDescription.Create(minLengthDescription);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(minLengthDescription);
        }

        [Fact]
        public void Create_Should_Handle_Max_Length_Description()
        {
            // Arrange
            var maxLengthDescription = new string('a', AdvertDescription.MaxLength);

            // Act
            var result = AdvertDescription.Create(maxLengthDescription);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(maxLengthDescription);
        }       

        [Fact]
        public void Create_Should_Succeed_When_Description_With_Special_Characters()
        {
            // Arrange
            var descriptionWithSymbols = "MacBook Pro 2023 - Excellent condition! Price: $1500. Contact: (555) 123-4567";

            // Act
            var result = AdvertDescription.Create(descriptionWithSymbols);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(descriptionWithSymbols);
        }

        [Fact]
        public void Create_Should_Succeed_When_Description_With_NewLines()
        {
            // Arrange
            var multilineDescription = "Features:\n- Brand new\n- Original box\n- 1 year warranty\n\nContact for details.";

            // Act
            var result = AdvertDescription.Create(multilineDescription);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(multilineDescription);
        }

        [Fact]
        public void Create_Should_Succeed_When_Description_With_Unicode_Characters()
        {
            // Arrange
            var unicodeDescription = "Ноутбук в отличном состоянии. Цена: 50000 руб. Торг уместен.";

            // Act
            var result = AdvertDescription.Create(unicodeDescription);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(unicodeDescription);
        }

        [Fact]
        public void Create_Should_Succeed_When_Description_With_Urls()
        {
            // Arrange
            var descriptionWithUrls = "Check photos: https://example.com/photos.jpg\nMore info: http://test.org";

            // Act
            var result = AdvertDescription.Create(descriptionWithUrls);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(descriptionWithUrls);
        }

        [Fact]
        public void Create_Should_Succeed_When_Description_With_Emojis()
        {
            // Arrange
            var descriptionWithEmojis = "🚗 Car for sale! Excellent condition 👍 Low mileage ⭐⭐⭐⭐⭐";

            // Act
            var result = AdvertDescription.Create(descriptionWithEmojis);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(descriptionWithEmojis);
        }

        [Fact]
        public void Create_Should_Succeed_With_Realistic_Long_Description()
        {
            // Arrange
            var realisticDescription = @"Selling my MacBook Pro 2023 with M2 chip. 
                Used for only 2 months, perfect condition. 
                Includes original box, charger, and all accessories.
                Never been repaired, no scratches or dents.
                Battery health: 100%
                Storage: 512GB SSD
                RAM: 16GB
                Perfect for work and creative tasks.
                Price is negotiable. Contact me for more details or to arrange a meeting.";

            // Act
            var result = AdvertDescription.Create(realisticDescription);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(realisticDescription);
        }
    }
}
