using FluentAssertions;
using SolarLab.AdvertBoard.Domain.Categories;
using SolarLab.AdvertBoard.Domain.Errors;

namespace SolarLab.AdvertBoard.UnitTests.Domain.Categories
{
    public class CategoryTitleTests
    {
        [Theory]
        [InlineData("Electronics")]
        [InlineData("Books")]
        [InlineData("Real Estate")]
        [InlineData("Automotive")]
        [InlineData("Home & Garden")]
        public void Create_Should_Succeed_When_Title_Valid(string validTitle)
        {
            // Act
            var result = CategoryTitle.Create(validTitle);

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
            var result = CategoryTitle.Create(emptyTitle);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CategoryErrors.Title.Empty);
        }

        [Theory]
        [InlineData("ab")] 
        [InlineData("a")]  
        public void Create_Should_Fail_When_Title_Too_Short(string shortTitle)
        {
            // Act
            var result = CategoryTitle.Create(shortTitle);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CategoryErrors.Title.TooShort);
        }

        [Fact]
        public void Create_Should_Fail_When_Title_Too_Long()
        {
            // Arrange
            var longTitle = new string('a', CategoryTitle.MaxLength + 1);

            // Act
            var result = CategoryTitle.Create(longTitle);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CategoryErrors.Title.TooLong);
        }

        [Fact]
        public void Create_Should_Fail_With_Correct_Error_For_Each_Validation()
        {
            // Test empty
            var emptyResult = CategoryTitle.Create("");
            emptyResult.IsFailure.Should().BeTrue();
            emptyResult.Error.Should().Be(CategoryErrors.Title.Empty);

            // Test too short
            var shortResult = CategoryTitle.Create("ab");
            shortResult.IsFailure.Should().BeTrue();
            shortResult.Error.Should().Be(CategoryErrors.Title.TooShort);

            // Test too long
            var longTitle = new string('a', CategoryTitle.MaxLength + 1);
            var longResult = CategoryTitle.Create(longTitle);
            longResult.IsFailure.Should().BeTrue();
            longResult.Error.Should().Be(CategoryErrors.Title.TooLong);
        }

        [Fact]
        public void Create_Should_Create_Different_Instances_For_Same_Value()
        {
            // Arrange
            var title = "Electronics";

            // Act
            var instance1 = CategoryTitle.Create(title).Value;
            var instance2 = CategoryTitle.Create(title).Value;

            // Assert
            instance1.Should().NotBeSameAs(instance2);
            instance1.Should().Be(instance2);
        }

        [Fact]
        public void Create_Should_Handle_Min_Length_Title()
        {
            // Arrange
            var minLengthTitle = new string('a', CategoryTitle.MinLength);

            // Act
            var result = CategoryTitle.Create(minLengthTitle);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(minLengthTitle);
        }

        [Fact]
        public void Create_Should_Handle_Max_Length_Title()
        {
            // Arrange
            var maxLengthTitle = new string('a', CategoryTitle.MaxLength);

            // Act
            var result = CategoryTitle.Create(maxLengthTitle);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(maxLengthTitle);
        }

        [Theory]
        [InlineData("Home & Garden")]
        [InlineData("Real Estate")]
        [InlineData("Sport & Fitness")]
        [InlineData("Beauty & Health")]
        public void Create_Should_Succeed_When_Title_With_Special_Characters(string validTitle)
        {
            // Act
            var result = CategoryTitle.Create(validTitle);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(validTitle);
        }

        [Fact]
        public void Create_Should_Succeed_When_Title_With_Numbers_And_Symbols()
        {
            // Arrange
            var titleWithSymbols = "Electronics & Gadgets";

            // Act
            var result = CategoryTitle.Create(titleWithSymbols);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(titleWithSymbols);
        }

        [Fact]
        public void Create_Should_Succeed_When_Title_With_Unicode_Characters()
        {
            // Arrange
            var unicodeTitle = "Электроника";

            // Act
            var result = CategoryTitle.Create(unicodeTitle);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(unicodeTitle);
        }

        [Fact]
        public void Create_Should_Succeed_With_Common_Category_Names()
        {
            var commonCategories = new[]
            {
                "Electronics",
                "Clothing",
                "Books",
                "Home",
                "Sports",
                "Toys",
                "Automotive",
                "Real Estate",
                "Jobs",
                "Services"
            };

            foreach (var category in commonCategories)
            {
                // Act
                var result = CategoryTitle.Create(category);

                // Assert
                result.IsSuccess.Should().BeTrue($"Category '{category}' should be valid");
                result.Value.Value.Should().Be(category);
            }
        }
    }
}
