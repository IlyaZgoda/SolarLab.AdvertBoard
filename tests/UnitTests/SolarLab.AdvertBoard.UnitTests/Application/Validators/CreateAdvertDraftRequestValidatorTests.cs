using AutoFixture;
using FluentValidation.TestHelper;
using SolarLab.AdvertBoard.Application.Adverts.CreateDraft;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Errors;

namespace SolarLab.AdvertBoard.UnitTests.Application.Validators
{
    public class CreateAdvertDraftRequestValidatorTests
    {
        private readonly CreateAdvertDraftRequestValidator _validator;
        private readonly Fixture _fixture;

        public CreateAdvertDraftRequestValidatorTests()
        {
            _validator = new CreateAdvertDraftRequestValidator();
            _fixture = new Fixture();
        }

        [Fact]
        public void Should_Not_Have_Errors_When_Valid()
        {
            // Arrange
            var request = _fixture
                .Build<CreateAdvertDraftRequest>()
                .With(x => x.Title, "Valid Title for Testing")
                .With(x => x.Description, "This is a valid description that meets all requirements for testing purposes.")
                .With(x => x.Price, 1000.50m)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        #region Title Validation Tests

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Should_Have_Error_When_Title_Is_Empty(string emptyTitle)
        {
            // Arrange
            var request = _fixture
                .Build<CreateAdvertDraftRequest>()
                .With(x => x.Title, emptyTitle)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Title)
                  .WithErrorMessage(AdvertErrors.Title.Empty.Description);
        }

        [Fact]
        public void Should_Have_Error_When_Title_Too_Short()
        {
            // Arrange
            var shortTitle = new string('a', AdvertTitle.MinLength - 1);
            var request = _fixture
                .Build<CreateAdvertDraftRequest>()
                .With(x => x.Title, shortTitle)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Title)
                  .WithErrorMessage(AdvertErrors.Title.TooShort.Description);
        }

        [Fact]
        public void Should_Have_Error_When_Title_Too_Long()
        {
            // Arrange
            var longTitle = new string('a', AdvertTitle.MaxLength + 1);
            var request = _fixture
                .Build<CreateAdvertDraftRequest>()
                .With(x => x.Title, longTitle)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Title)
                  .WithErrorMessage(AdvertErrors.Title.TooLong.Description);
        }

        [Theory]
        [InlineData(AdvertTitle.MinLength)]
        [InlineData(AdvertTitle.MinLength + 1)]
        [InlineData(AdvertTitle.MaxLength - 1)]
        [InlineData(AdvertTitle.MaxLength)]
        public void Should_Not_Have_Error_When_Title_Length_Valid(int titleLength)
        {
            // Arrange
            var validTitle = new string('a', titleLength);
            var request = _fixture
                .Build<CreateAdvertDraftRequest>()
                .With(x => x.Title, validTitle)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Title);
        }

        #endregion

        #region Description Validation Tests

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Should_Have_Error_When_Description_Is_Empty(string emptyDescription)
        {
            // Arrange
            var request = _fixture
                .Build<CreateAdvertDraftRequest>()
                .With(x => x.Description, emptyDescription)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Description)
                  .WithErrorMessage(AdvertErrors.Description.Empty.Description);
        }

        [Fact]
        public void Should_Have_Error_When_Description_Too_Short()
        {
            // Arrange
            var shortDescription = new string('a', AdvertDescription.MinLength - 1);
            var request = _fixture
                .Build<CreateAdvertDraftRequest>()
                .With(x => x.Description, shortDescription)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Description)
                  .WithErrorMessage(AdvertErrors.Description.TooShort.Description);
        }

        [Fact]
        public void Should_Have_Error_When_Description_Too_Long()
        {
            // Arrange
            var longDescription = new string('a', AdvertDescription.MaxLength + 1);
            var request = _fixture
                .Build<CreateAdvertDraftRequest>()
                .With(x => x.Description, longDescription)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Description)
                  .WithErrorMessage(AdvertErrors.Description.TooLong.Description);
        }

        [Theory]
        [InlineData(AdvertDescription.MinLength)]
        [InlineData(AdvertDescription.MinLength + 1)]
        [InlineData(AdvertDescription.MaxLength - 1)]
        [InlineData(AdvertDescription.MaxLength)]
        public void Should_Not_Have_Error_When_Description_Length_Valid(int descriptionLength)
        {
            // Arrange
            var validDescription = new string('a', descriptionLength);
            var request = _fixture
                .Build<CreateAdvertDraftRequest>()
                .With(x => x.Description, validDescription)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Description);
        }

        #endregion

        #region Price Validation Tests

        [Fact]
        public void Should_Have_Error_When_Price_Too_Low()
        {
            // Arrange
            var request = _fixture
                .Build<CreateAdvertDraftRequest>()
                .With(x => x.Price, Price.MinValue - 1)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Price)
                  .WithErrorMessage(AdvertErrors.Price.TooLow.Description);
        }

        [Fact]
        public void Should_Have_Error_When_Price_Too_High()
        {
            // Arrange
            var request = _fixture
                .Build<CreateAdvertDraftRequest>()
                .With(x => x.Price, Price.MaxValue + 1)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Price)
                  .WithErrorMessage(AdvertErrors.Price.TooHigh.Description);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Price_Valid()
        {
            // Arrange
            var request = _fixture
                .Build<CreateAdvertDraftRequest>()
                .With(x => x.Price, Price.MinValue + 1)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Price);
        }

        #endregion

        #region Combined Validation Tests

        [Fact]
        public void Should_Have_Multiple_Errors_When_Multiple_Fields_Invalid()
        {
            // Arrange
            var longTitle = new string('a', AdvertTitle.MaxLength + 1);
            var longDescription = new string('a', AdvertDescription.MaxLength + 1);
            var request = _fixture
                .Build<CreateAdvertDraftRequest>()
                .With(x => x.Title, longTitle)
                .With(x => x.Description, longDescription)
                .With(x => x.Price, Price.MaxValue + 1)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Title);
            result.ShouldHaveValidationErrorFor(x => x.Description);
            result.ShouldHaveValidationErrorFor(x => x.Price);
            Assert.Equal(3, result.Errors.Count);
        }

        [Fact]
        public void Should_Validate_Realistic_Advert_Scenario()
        {
            // Arrange
            var request = _fixture
                .Build<CreateAdvertDraftRequest>()
                .With(x => x.Title, "MacBook Pro 2023 - Like New Condition")
                .With(x => x.Description, "Selling my MacBook Pro 2023 with M2 chip. Used for only 2 months, perfect condition. Includes original box and accessories.")
                .With(x => x.Price, 2500.00m)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        #endregion
    }
}
