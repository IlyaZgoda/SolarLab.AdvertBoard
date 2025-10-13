using AutoFixture;
using FluentValidation.TestHelper;
using SolarLab.AdvertBoard.Application.Adverts.UpdateDraft;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Errors;

namespace SolarLab.AdvertBoard.UnitTests.Application.Validators
{
    public class UpdateAdvertDraftRequestValidatorTests
    {
        private readonly UpdateAdvertDraftRequestValidator _validator;
        private readonly Fixture _fixture;

        public UpdateAdvertDraftRequestValidatorTests()
        {
            _validator = new UpdateAdvertDraftRequestValidator();
            _fixture = new Fixture();
        }

        [Fact]
        public void Should_Not_Have_Errors_When_All_Fields_Valid()
        {
            // Arrange
            var request = _fixture
                .Build<UpdateAdvertDraftRequest>()
                .With(x => x.Title, "Valid Title for Testing")
                .With(x => x.Description, "This is a valid description that meets all requirements for testing purposes.")
                .With(x => x.Price, 1000.50m)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_Not_Have_Errors_When_All_Fields_Null()
        {
            // Arrange
            var request = _fixture
                .Build<UpdateAdvertDraftRequest>()
                .Without(x => x.Title)
                .Without(x => x.Description)
                .Without(x => x.Price)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        #region Title Validation Tests (с условиями)

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Should_Not_Validate_Title_When_Title_Empty(string emptyTitle)
        {
            // Arrange
            var request = _fixture
                .Build<UpdateAdvertDraftRequest>()
                .With(x => x.Title, emptyTitle)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert 
            result.ShouldNotHaveValidationErrorFor(x => x.Title);
        }

        [Fact]
        public void Should_Not_Validate_Title_When_Title_Null()
        {
            // Arrange
            var request = _fixture
                .Build<UpdateAdvertDraftRequest>()
                .Without(x => x.Title)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Title);
        }

        [Fact]
        public void Should_Validate_Title_When_Title_Provided_And_Too_Short()
        {
            // Arrange
            var shortTitle = new string('a', AdvertTitle.MinLength - 1);
            var request = _fixture
                .Build<UpdateAdvertDraftRequest>()
                .With(x => x.Title, shortTitle)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Title)
                  .WithErrorMessage(AdvertErrors.Title.TooShort.Description);
        }

        [Fact]
        public void Should_Validate_Title_When_Title_Provided_And_Too_Long()
        {
            // Arrange
            var longTitle = new string('a', AdvertTitle.MaxLength + 1);
            var request = _fixture
                .Build<UpdateAdvertDraftRequest>()
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
        public void Should_Not_Have_Error_When_Title_Provided_And_Length_Valid(int titleLength)
        {
            // Arrange
            var validTitle = new string('a', titleLength);
            var request = _fixture
                .Build<UpdateAdvertDraftRequest>()
                .With(x => x.Title, validTitle)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Title);
        }

        #endregion

        #region Description Validation Tests (с условиями)

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Should_Not_Validate_Description_When_Description_Empty(string emptyDescription)
        {
            // Arrange
            var request = _fixture
                .Build<UpdateAdvertDraftRequest>()
                .With(x => x.Description, emptyDescription)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void Should_Not_Validate_Description_When_Description_Null()
        {
            // Arrange
            var request = _fixture
                .Build<UpdateAdvertDraftRequest>()
                .Without(x => x.Description)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void Should_Validate_Description_When_Description_Provided_And_Too_Short()
        {
            // Arrange
            var shortDescription = new string('a', AdvertDescription.MinLength - 1);
            var request = _fixture
                .Build<UpdateAdvertDraftRequest>()
                .With(x => x.Description, shortDescription)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Description)
                  .WithErrorMessage(AdvertErrors.Description.TooShort.Description);
        }

        [Fact]
        public void Should_Validate_Description_When_Description_Provided_And_Too_Long()
        {
            // Arrange
            var longDescription = new string('a', AdvertDescription.MaxLength + 1);
            var request = _fixture
                .Build<UpdateAdvertDraftRequest>()
                .With(x => x.Description, longDescription)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Description)
                  .WithErrorMessage(AdvertErrors.Description.TooLong.Description);
        }

        #endregion

        #region Price Validation Tests (с условиями)

        [Fact]
        public void Should_Not_Validate_Price_When_Price_Null()
        {
            // Arrange
            var request = _fixture
                .Build<UpdateAdvertDraftRequest>()
                .Without(x => x.Price)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Price);
        }

        [Fact]
        public void Should_Validate_Price_When_Price_Provided_And_Too_Low()
        {
            // Arrange
            var request = _fixture
                .Build<UpdateAdvertDraftRequest>()
                .With(x => x.Price, Price.MinValue - 1)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Price)
                  .WithErrorMessage(AdvertErrors.Price.TooLow.Description);
        }

        [Fact]
        public void Should_Validate_Price_When_Price_Provided_And_Too_High()
        {
            // Arrange
            var request = _fixture
                .Build<UpdateAdvertDraftRequest>()
                .With(x => x.Price, Price.MaxValue + 1)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Price)
                  .WithErrorMessage(AdvertErrors.Price.TooHigh.Description);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Price_Provided_And_Valid()
        {
            // Arrange
            var request = _fixture
                .Build<UpdateAdvertDraftRequest>()
                .With(x => x.Price, Price.MinValue + 1)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Price);
        }

        #endregion

        #region Combined Scenarios Tests (уникальные для Update)

        [Fact]
        public void Should_Validate_Only_Provided_Fields_And_Ignore_Others()
        {
            // Arrange
            var request = _fixture
                .Build<UpdateAdvertDraftRequest>()
                .With(x => x.Title, "Sh")
                .Without(x => x.Description)
                .With(x => x.Price, -100.0m)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Title);
            result.ShouldNotHaveValidationErrorFor(x => x.Description);
            result.ShouldHaveValidationErrorFor(x => x.Price);
        }

        [Fact]
        public void Should_Pass_When_Partial_Update_With_Valid_Data()
        {
            // Arrange
            var request = _fixture
                .Build<UpdateAdvertDraftRequest>()
                .With(x => x.Title, "Updated Title Only")
                .Without(x => x.Description)
                .Without(x => x.Price)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_Pass_When_Only_One_Field_Provided_And_Valid()
        {
            // Arrange
            var request = _fixture
                .Build<UpdateAdvertDraftRequest>()
                .With(x => x.Title, "Valid Updated Title")
                .Without(x => x.Description)
                .Without(x => x.Price)
                .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
            result.ShouldNotHaveValidationErrorFor(x => x.Description);
            result.ShouldNotHaveValidationErrorFor(x => x.Price);
        }

        [Fact]
        public void Should_Have_Multiple_Errors_When_Multiple_Provided_Fields_Invalid()
        {
            // Arrange
            var longTitle = new string('a', AdvertTitle.MaxLength + 1);
            var longDescription = new string('a', AdvertDescription.MaxLength + 1);
            var request = _fixture
                .Build<UpdateAdvertDraftRequest>()
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
        }

        #endregion
    }
}