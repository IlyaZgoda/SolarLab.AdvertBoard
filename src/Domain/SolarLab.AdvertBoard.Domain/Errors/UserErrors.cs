using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Domain.Errors
{
   public static class UserErrors
    {
        public static readonly Error NotFound = new(ErrorTypes.NotFound, "User not found");
        public static readonly Error CannotChangePassword = new(ErrorTypes.ValidationError, "Cannot change password");
        public static class FirstName
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, "First name is empty");
            public static readonly Error NotValid = new(ErrorTypes.ValidationError, "First name is not valid");
            public static readonly Error TooLong = new(ErrorTypes.ValidationError, "First name too long");
        }

        public static class LastName
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, "Last name is empty");
            public static readonly Error NotValid = new(ErrorTypes.ValidationError, "Last name is not valid");
            public static readonly Error TooLong = new(ErrorTypes.ValidationError, "Last name too long");
        }

        public static class MiddleName
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, "Middle name is empty");
            public static readonly Error NotValid = new(ErrorTypes.ValidationError, "Middle name is not valid");
            public static readonly Error TooLong = new(ErrorTypes.ValidationError, "Middle name too long");
        }

        public static class Email
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, "ContactEmail is empty");
            public static readonly Error NotValid = new(ErrorTypes.ValidationError, "ContactEmail is not valid");
            public static readonly Error TooLong = new(ErrorTypes.ValidationError, "ContactEmail too long");
        }

        public static class PhoneNumber
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, "Phone number is empty");
            public static readonly Error NotValid = new(ErrorTypes.ValidationError, "Phone number is not valid");
            public static readonly Error TooLong = new(ErrorTypes.ValidationError, "Phone number too long");
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
