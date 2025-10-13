using AutoFixture;
using FluentValidation.TestHelper;
using SolarLab.AdvertBoard.Application.Users.Register;
using SolarLab.AdvertBoard.Contracts.Authentication;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.Domain.Users;

namespace SolarLab.AdvertBoard.UnitTests.Application.Validators
{
    public class RegisterUserRequestValidatorTests
    {
        private readonly RegisterUserRequestValidator _validator;
        private readonly Fixture _fixture;

        public RegisterUserRequestValidatorTests()
        {
            _validator = new RegisterUserRequestValidator();
            _fixture = new Fixture();
        }

        [Fact]
        public void Should_Not_Have_Errors_When_All_Fields_Valid()
        {
            // Arrange
            var request = _fixture
               .Build<RegisterUserRequest>()
               .With(x => x.Email, "Test@mail.ru")
               .With(x => x.ContactEmail, "Test@mail.ru")
               .With(x => x.Password, "ValidPass123!")
               .With(x => x.FirstName, "John")
               .With(x => x.LastName, "Doe")
               .With(x => x.MiddleName, "Michael")
               .With(x => x.PhoneNumber, "+79039495566")
               .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_Not_Have_Errors_When_Optional_Fields_Empty()
        {
            // Arrange
            var request = _fixture
              .Build<RegisterUserRequest>()
              .With(x => x.Email, "Test@mail.ru")
              .With(x => x.ContactEmail, "")
              .With(x => x.Password, "ValidPass123!")
              .With(x => x.FirstName, "John")
              .With(x => x.LastName, "Doe")
              .With(x => x.MiddleName, "")
              .With(x => x.PhoneNumber, "")
              .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        #region Email Validation Tests

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Should_Have_Error_When_Email_Empty(string emptyEmail)
        {
            // Arrange
            var request = _fixture
              .Build<RegisterUserRequest>()
              .With(x => x.Email, emptyEmail)
              .With(x => x.ContactEmail, "Test@mail.ru")
              .With(x => x.Password, "ValidPass123!")
              .With(x => x.FirstName, "John")
              .With(x => x.LastName, "Doe")
              .With(x => x.MiddleName, "Michael")
              .With(x => x.PhoneNumber, "+79039495566")
              .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorMessage("Email is required");
        }

        [Theory]
        [InlineData("invalid-email")]
        [InlineData("invalid.email")]
        [InlineData("@domain.com")]
        [InlineData("test@")]
        public void Should_Have_Error_When_Email_Invalid(string invalidEmail)
        {
            // Arrange
            var request = _fixture
              .Build<RegisterUserRequest>()
              .With(x => x.Email, invalidEmail)
              .With(x => x.ContactEmail, "Test@mail.ru")
              .With(x => x.Password, "ValidPass123!")
              .With(x => x.FirstName, "John")
              .With(x => x.LastName, "Doe")
              .With(x => x.MiddleName, "Michael")
              .With(x => x.PhoneNumber, "+79039495566")
              .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorMessage("Invalid email format");
        }

        [Fact]
        public void Should_Have_Error_When_Email_Too_Long()
        {
            // Arrange
            var longEmail = new string('a', 310) + "@example.com"; 
            var request = _fixture
              .Build<RegisterUserRequest>()
              .With(x => x.Email, longEmail)
              .With(x => x.ContactEmail, "Test@mail.ru")
              .With(x => x.Password, "ValidPass123!")
              .With(x => x.FirstName, "John")
              .With(x => x.LastName, "Doe")
              .With(x => x.MiddleName, "Michael")
              .With(x => x.PhoneNumber, "+79039495566")
              .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorMessage("Email must not exceed 320 characters");
        }

        #endregion

        #region Password Validation Tests

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Should_Have_Error_When_Password_Empty(string emptyPassword)
        {
            // Arrange
            var request = _fixture
               .Build<RegisterUserRequest>()
               .With(x => x.Email, "Test@mail.ru")
               .With(x => x.ContactEmail, "Test@mail.ru")
               .With(x => x.Password, emptyPassword)
               .With(x => x.FirstName, "John")
               .With(x => x.LastName, "Doe")
               .With(x => x.MiddleName, "Michael")
               .With(x => x.PhoneNumber, "+79039495566")
               .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                  .WithErrorMessage("Password is required");
        }

        [Theory]
        [InlineData("123")]
        [InlineData("12")]
        [InlineData("1")]
        public void Should_Have_Error_When_Password_Too_Short(string shortPassword)
        {
            // Arrange
            var request = _fixture
              .Build<RegisterUserRequest>()
              .With(x => x.Email, "Test@mail.ru")
              .With(x => x.ContactEmail, "Test@mail.ru")
              .With(x => x.Password, shortPassword)
              .With(x => x.FirstName, "John")
              .With(x => x.LastName, "Doe")
              .With(x => x.MiddleName, "Michael")
              .With(x => x.PhoneNumber, "+79039495566")
              .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                  .WithErrorMessage("Password must be at least 6 characters");
        }

        [Fact]
        public void Should_Have_Error_When_Password_Too_Long()
        {
            // Arrange
            var longPassword = new string('a', 101);
            var request = _fixture
              .Build<RegisterUserRequest>()
              .With(x => x.Email, "Test@mail.ru")
              .With(x => x.ContactEmail, "Test@mail.ru")
              .With(x => x.Password, longPassword)
              .With(x => x.FirstName, "John")
              .With(x => x.LastName, "Doe")
              .With(x => x.MiddleName, "Michael")
              .With(x => x.PhoneNumber, "+79039495566")
              .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                  .WithErrorMessage("Password must not exceed 100 characters");
        }

        [Theory]
        [InlineData("nouppercase123!")]
        [InlineData("NOLOWERCASE123!")]
        [InlineData("NoDigitsHere!")]
        [InlineData("NoSpecialChar123")]
        public void Should_Have_Error_When_Password_Missing_Requirements(string invalidPassword)
        {
            // Arrange
            var request = _fixture
              .Build<RegisterUserRequest>()
              .With(x => x.Email, "Test@mail.ru")
              .With(x => x.ContactEmail, "Test@mail.ru")
              .With(x => x.Password, invalidPassword)
              .With(x => x.FirstName, "John")
              .With(x => x.LastName, "Doe")
              .With(x => x.MiddleName, "Michael")
              .With(x => x.PhoneNumber, "+79039495566")
              .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Theory]
        [InlineData("ValidPass123!")]
        [InlineData("Another$Valid1")]
        [InlineData("Test@Password123")]
        public void Should_Not_Have_Error_When_Password_Meets_All_Requirements(string validPassword)
        {
            // Arrange
            var request = _fixture
              .Build<RegisterUserRequest>()
              .With(x => x.Email, "Test@mail.ru")
              .With(x => x.ContactEmail, "Test@mail.ru")
              .With(x => x.Password, validPassword)
              .With(x => x.FirstName, "John")
              .With(x => x.LastName, "Doe")
              .With(x => x.MiddleName, "Michael")
              .With(x => x.PhoneNumber, "+79039495566")
              .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }

        #endregion

        #region FirstName Validation Tests

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Should_Have_Error_When_FirstName_Empty(string emptyFirstName)
        {
            // Arrange
            var request = _fixture
              .Build<RegisterUserRequest>()
              .With(x => x.Email, "Test@mail.ru")
              .With(x => x.ContactEmail, "Test@mail.ru")
              .With(x => x.Password, "ValidPass123!")
              .With(x => x.FirstName, emptyFirstName)
              .With(x => x.LastName, "Doe")
              .With(x => x.MiddleName, "Michael")
              .With(x => x.PhoneNumber, "+79039495566")
              .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                  .WithErrorMessage(UserErrors.FirstName.Empty.Description);
        }

        [Fact]
        public void Should_Have_Error_When_FirstName_Too_Long()
        {
            // Arrange
            var longFirstName = new string('a', FirstName.MaxLength + 1);
            var request = _fixture
              .Build<RegisterUserRequest>()
              .With(x => x.Email, "Test@mail.ru")
              .With(x => x.ContactEmail, "Test@mail.ru")
              .With(x => x.Password, "ValidPass123!")
              .With(x => x.FirstName, longFirstName)
              .With(x => x.LastName, "Doe")
              .With(x => x.MiddleName, "Michael")
              .With(x => x.PhoneNumber, "+79039495566")
              .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                  .WithErrorMessage(UserErrors.FirstName.TooLong.Description);
        }

        [Theory]
        [InlineData("John123")] 
        [InlineData("John@Doe")] 
        [InlineData("John_Doe")] 
        public void Should_Have_Error_When_FirstName_Invalid_Format(string invalidFirstName)
        {
            // Arrange
            var request = _fixture
              .Build<RegisterUserRequest>()
              .With(x => x.Email, "Test@mail.ru")
              .With(x => x.ContactEmail, "Test@mail.ru")
              .With(x => x.Password, "ValidPass123!")
              .With(x => x.FirstName, invalidFirstName)
              .With(x => x.LastName, "Doe")
              .With(x => x.MiddleName, "Michael")
              .With(x => x.PhoneNumber, "+79039495566")
              .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                  .WithErrorMessage(UserErrors.FirstName.NotValid.Description);
        }

        [Theory]
        [InlineData("John")]
        [InlineData("Mary")]
        [InlineData("Jean-Claude")]
        [InlineData("O'Neill")]
        public void Should_Not_Have_Error_When_FirstName_Valid(string validFirstName)
        {
            // Arrange
            var request = _fixture
              .Build<RegisterUserRequest>()
              .With(x => x.Email, "Test@mail.ru")
              .With(x => x.ContactEmail, "Test@mail.ru")
              .With(x => x.Password, "ValidPass123!")
              .With(x => x.FirstName, validFirstName)
              .With(x => x.LastName, "Doe")
              .With(x => x.MiddleName, "Michael")
              .With(x => x.PhoneNumber, "+79039495566")
              .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.FirstName);
        }

        #endregion

        #region LastName Validation Tests

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Should_Have_Error_When_LastName_Empty(string emptyLastName)
        {
            // Arrange
            var request = _fixture
              .Build<RegisterUserRequest>()
              .With(x => x.Email, "Test@mail.ru")
              .With(x => x.ContactEmail, "Test@mail.ru")
              .With(x => x.Password, "ValidPass123!")
              .With(x => x.FirstName, "John")
              .With(x => x.LastName, emptyLastName)
              .With(x => x.MiddleName, "Michael")
              .With(x => x.PhoneNumber, "+79039495566")
              .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.LastName)
                  .WithErrorMessage(UserErrors.LastName.Empty.Description);
        }

        [Fact]
        public void Should_Have_Error_When_LastName_Too_Long()
        {
            // Arrange
            var longLastName = new string('a', LastName.MaxLength + 1);
            var request = _fixture
              .Build<RegisterUserRequest>()
              .With(x => x.Email, "Test@mail.ru")
              .With(x => x.ContactEmail, "Test@mail.ru")
              .With(x => x.Password, "ValidPass123!")
              .With(x => x.FirstName, "John")
              .With(x => x.LastName, longLastName)
              .With(x => x.MiddleName, "Michael")
              .With(x => x.PhoneNumber, "+79039495566")
              .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.LastName)
                  .WithErrorMessage(UserErrors.LastName.TooLong.Description);
        }

        #endregion

        #region MiddleName Validation Tests (Optional)

        [Fact]
        public void Should_Not_Validate_MiddleName_When_Empty()
        {
            // Arrange
            var request = _fixture
               .Build<RegisterUserRequest>()
               .With(x => x.Email, "Test@mail.ru")
               .With(x => x.ContactEmail, "Test@mail.ru")
               .With(x => x.Password, "ValidPass123!")
               .With(x => x.FirstName, "John")
               .With(x => x.LastName, "Doe")
               .With(x => x.MiddleName, "")
               .With(x => x.PhoneNumber, "+79039495566")
               .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.MiddleName);
        }

        [Fact]
        public void Should_Validate_MiddleName_When_Provided_And_Too_Long()
        {
            // Arrange
            var longMiddleName = new string('a', MiddleName.MaxLength + 1);
            var request = _fixture
               .Build<RegisterUserRequest>()
               .With(x => x.Email, "Test@mail.ru")
               .With(x => x.ContactEmail, "Test@mail.ru")
               .With(x => x.Password, "ValidPass123!")
               .With(x => x.FirstName, "John")
               .With(x => x.LastName, "Doe")
               .With(x => x.MiddleName, longMiddleName)
               .With(x => x.PhoneNumber, "+79039495566")
               .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.MiddleName)
                  .WithErrorMessage(UserErrors.MiddleName.Empty.Description); 
        }

        [Theory]
        [InlineData("ValidMiddle")]
        [InlineData("Van")]
        [InlineData("De")]
        public void Should_Not_Have_Error_When_MiddleName_Provided_And_Valid(string validMiddleName)
        {
            // Arrange
            var request = _fixture
               .Build<RegisterUserRequest>()
               .With(x => x.Email, "Test@mail.ru")
               .With(x => x.ContactEmail, "Test@mail.ru")
               .With(x => x.Password, "ValidPass123!")
               .With(x => x.FirstName, "John")
               .With(x => x.LastName, "Doe")
               .With(x => x.MiddleName, validMiddleName)
               .With(x => x.PhoneNumber, "+79039495566")
               .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.MiddleName);
        }

        #endregion

        #region ContactEmail Validation Tests (Optional)

        [Fact]
        public void Should_Not_Validate_ContactEmail_When_Empty()
        {
            // Arrange
            var request = _fixture
              .Build<RegisterUserRequest>()
              .With(x => x.Email, "Test@mail.ru")
              .With(x => x.ContactEmail, "")
              .With(x => x.Password, "ValidPass123!")
              .With(x => x.FirstName, "John")
              .With(x => x.LastName, "Doe")
              .With(x => x.MiddleName, "Michael")
              .With(x => x.PhoneNumber, "+79039495566")
              .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.ContactEmail);
        }

        [Theory]
        [InlineData("invalid-email")]
        [InlineData("invalid.email")]
        public void Should_Validate_ContactEmail_When_Provided_And_Invalid(string invalidContactEmail)
        {
            // Arrange
            var request = _fixture
              .Build<RegisterUserRequest>()
              .With(x => x.Email, "Test@mail.ru")
              .With(x => x.ContactEmail, invalidContactEmail)
              .With(x => x.Password, "ValidPass123!")
              .With(x => x.FirstName, "John")
              .With(x => x.LastName, "Doe")
              .With(x => x.MiddleName, "Michael")
              .With(x => x.PhoneNumber, "+79039495566")
              .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ContactEmail)
                  .WithErrorMessage("Invalid email format");
        }

        [Fact]
        public void Should_Validate_ContactEmail_When_Provided_And_Too_Long()
        {
            // Arrange
            var longContactEmail = new string('a', 310) + "@example.com";
            var request = _fixture
              .Build<RegisterUserRequest>()
              .With(x => x.Email, "Test@mail.ru")
              .With(x => x.ContactEmail, longContactEmail)
              .With(x => x.Password, "ValidPass123!")
              .With(x => x.FirstName, "John")
              .With(x => x.LastName, "Doe")
              .With(x => x.MiddleName, "Michael")
              .With(x => x.PhoneNumber, "+79039495566")
              .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ContactEmail)
                  .WithErrorMessage("Contact email must not exceed 320 characters");
        }

        #endregion

        #region PhoneNumber Validation Tests (Optional)

        [Fact]
        public void Should_Not_Validate_PhoneNumber_When_Empty()
        {
            // Arrange
            var request = _fixture
              .Build<RegisterUserRequest>()
              .With(x => x.Email, "Test@mail.ru")
              .With(x => x.ContactEmail, "Test@mail.ru")
              .With(x => x.Password, "ValidPass123!")
              .With(x => x.FirstName, "John")
              .With(x => x.LastName, "Doe")
              .With(x => x.MiddleName, "Michael")
              .With(x => x.PhoneNumber, "")
              .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
        }

        [Fact]
        public void Should_Validate_PhoneNumber_When_Provided_And_Too_Long()
        {
            // Arrange
            var longPhoneNumber = new string('1', PhoneNumber.MaxLength + 1);
            var request = _fixture
              .Build<RegisterUserRequest>()
              .With(x => x.Email, "Test@mail.ru")
              .With(x => x.ContactEmail, "Test@mail.ru")
              .With(x => x.Password, "ValidPass123!")
              .With(x => x.FirstName, "John")
              .With(x => x.LastName, "Doe")
              .With(x => x.MiddleName, "Michael")
              .With(x => x.PhoneNumber, longPhoneNumber)
              .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
                  .WithErrorMessage(UserErrors.PhoneNumber.TooLong.Description);
        }

        [Theory]
        [InlineData("invalid-phone")]
        [InlineData("abc123")]
        [InlineData("+123")]
        public void Should_Validate_PhoneNumber_When_Provided_And_Invalid_Format(string invalidPhoneNumber)
        {
            // Arrange
            var request = _fixture
              .Build<RegisterUserRequest>()
              .With(x => x.Email, "Test@mail.ru")
              .With(x => x.ContactEmail, "Test@mail.ru")
              .With(x => x.Password, "ValidPass123!")
              .With(x => x.FirstName, "John")
              .With(x => x.LastName, "Doe")
              .With(x => x.MiddleName, "Michael")
              .With(x => x.PhoneNumber, invalidPhoneNumber)
              .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
                  .WithErrorMessage(UserErrors.PhoneNumber.NotValid.Description);
        }

        #endregion

        #region Combined Scenarios Tests

        [Fact]
        public void Should_Have_Multiple_Errors_When_Multiple_Required_Fields_Invalid()
        {
            // Arrange
            var request = _fixture
              .Build<RegisterUserRequest>()
              .With(x => x.Email, "Test-mail.ru")
              .With(x => x.ContactEmail, "")
              .With(x => x.Password, "123")
              .With(x => x.FirstName, "John123")
              .With(x => x.LastName, "")
              .With(x => x.MiddleName, "")
              .With(x => x.PhoneNumber, "")
              .Create();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email);
            result.ShouldHaveValidationErrorFor(x => x.Password);
            result.ShouldHaveValidationErrorFor(x => x.FirstName);
            result.ShouldHaveValidationErrorFor(x => x.LastName);
        }

        #endregion
    }
}