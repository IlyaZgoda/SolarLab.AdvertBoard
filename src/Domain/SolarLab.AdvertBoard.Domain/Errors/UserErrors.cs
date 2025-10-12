using SolarLab.AdvertBoard.SharedKernel;
using UserFirstName = SolarLab.AdvertBoard.Domain.Users.FirstName;
using UserLastName = SolarLab.AdvertBoard.Domain.Users.LastName;
using UserMiddleName = SolarLab.AdvertBoard.Domain.Users.MiddleName;
using UserEmail = SolarLab.AdvertBoard.Domain.Users.ContactEmail;
using UserPhoneNumber = SolarLab.AdvertBoard.Domain.Users.PhoneNumber;

namespace SolarLab.AdvertBoard.Domain.Errors
{
   public static class UserErrors
    {
        public static readonly Error NotFound = new(ErrorTypes.NotFound, "User not found");
        public static readonly Error CannotChangePassword = new(ErrorTypes.ValidationError, "Cannot change password");
        public static class FirstName
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, "First name is required");
            public static readonly Error NotValid = new(ErrorTypes.ValidationError, "First name can only contain letters, spaces and hyphens");
            public static readonly Error TooLong = new(ErrorTypes.ValidationError, $"First name must not exceed {UserFirstName.MaxLength}");
        }

        public static class LastName
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, "Last name is required");
            public static readonly Error NotValid = new(ErrorTypes.ValidationError, "Last name can only contain letters, spaces and hyphens");
            public static readonly Error TooLong = new(ErrorTypes.ValidationError, $"First name must not exceed {UserFirstName.MaxLength}");
        }

        public static class MiddleName
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, "Middle name is required");
            public static readonly Error NotValid = new(ErrorTypes.ValidationError, "Middle name can only contain letters, spaces and hyphens");
            public static readonly Error TooLong = new(ErrorTypes.ValidationError, $"First name must not exceed {UserFirstName.MaxLength}");
        }

        public static class Email
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, "Contact email is required");
            public static readonly Error NotValid = new(ErrorTypes.ValidationError, "Contact email has invalid format");
            public static readonly Error TooLong = new(ErrorTypes.ValidationError, $"Contact email must not exceed {UserEmail.MaxLength}");
        }

        public static class PhoneNumber
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, "Phone number is required");
            public static readonly Error NotValid = new(ErrorTypes.ValidationError, "Phone number has invalid format");
            public static readonly Error TooLong = new(ErrorTypes.ValidationError, $"Phone number must not exceed {UserPhoneNumber.MaxLength}");
        }

        public static class PasswordHash
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, "Password hash is empty");
        }

        public static class Password
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, "Password is empty");
            public static readonly Error TooShort = new(ErrorTypes.ValidationError, "Password is shorter than min length");
            public static readonly Error TooLong = new(ErrorTypes.ValidationError, "Password is longer than max length");
            public static readonly Error NotValid = new(ErrorTypes.ValidationError, "Password has invalid format");
            public static readonly Error MissingDigits = new(ErrorTypes.ValidationError, "Password has no digits");
            public static readonly Error MissingLowercaseLetters = new(ErrorTypes.ValidationError, "Password has no lowercase letters");
            public static readonly Error MissingUppercaseLetters = new(ErrorTypes.ValidationError, "Password has no uppercase letters");
            public static readonly Error MissingSpecialCharacters = new(ErrorTypes.ValidationError, "Password has no special characters");
        }
    }
}
