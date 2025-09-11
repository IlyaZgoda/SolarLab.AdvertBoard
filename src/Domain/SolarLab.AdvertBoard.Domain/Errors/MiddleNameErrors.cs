using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Domain.Errors
{
    public static class FirstNameErrors
    {
        public static readonly Error Empty = new(ErrorTypes.ValidationError, "First name is empty");
        public static readonly Error NotValid = new(ErrorTypes.ValidationError, "First name is not valid");
        public static readonly Error TooLong = new(ErrorTypes.ValidationError, "First name too long");
    }

    public static class LastNameErrors
    {
        public static readonly Error Empty = new(ErrorTypes.ValidationError, "Last name is empty");
        public static readonly Error NotValid = new(ErrorTypes.ValidationError, "Last name is not valid");
        public static readonly Error TooLong = new(ErrorTypes.ValidationError, "Last name too long");
    }

    public static class MiddleNameErrors
    {
        public static readonly Error Empty = new(ErrorTypes.ValidationError, "Middle name is empty");
        public static readonly Error NotValid = new(ErrorTypes.ValidationError, "Middle name is not valid");
        public static readonly Error TooLong = new(ErrorTypes.ValidationError, "Middle name too long");
    }

    public static class EmailErrors
    {
        public static readonly Error Empty = new(ErrorTypes.ValidationError, "Email is empty");
        public static readonly Error NotValid = new(ErrorTypes.ValidationError, "Email is not valid");
        public static readonly Error TooLong = new(ErrorTypes.ValidationError, "Email too long");
    }

    public static class PhoneNumberErrors
    {
        public static readonly Error Empty = new(ErrorTypes.ValidationError, "Phone number is empty");
        public static readonly Error NotValid = new(ErrorTypes.ValidationError, "Phone number is not valid");
        public static readonly Error TooLong = new(ErrorTypes.ValidationError, "Phone number too long");
    }

    public static class PasswordHashErrors
    {
        public static readonly Error Empty = new(ErrorTypes.ValidationError, "Password hash is empty");
    }

    public static class PasswordErrors
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
