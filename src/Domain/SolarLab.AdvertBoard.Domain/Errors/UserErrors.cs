using SolarLab.AdvertBoard.SharedKernel;
using UserFirstName = SolarLab.AdvertBoard.Domain.Users.FirstName;
using UserLastName = SolarLab.AdvertBoard.Domain.Users.LastName;
using UserMiddleName = SolarLab.AdvertBoard.Domain.Users.MiddleName;
using UserEmail = SolarLab.AdvertBoard.Domain.Users.ContactEmail;
using UserPhoneNumber = SolarLab.AdvertBoard.Domain.Users.PhoneNumber;

namespace SolarLab.AdvertBoard.Domain.Errors
{
    /// <summary>
    /// Статический класс, содержащий ошибки, связанные с операциями над пользователем системы
    /// </summary>
    public static class UserErrors
    {
        public static readonly Error NotFound = new(ErrorTypes.NotFound, "User not found");
        public static readonly Error CannotChangePassword = new(ErrorTypes.ValidationError, "Cannot change password");

        /// <summary>
        /// Группа ошибок, связанных с именем пользователя
        /// </summary>
        public static class FirstName
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, "First name is required");
            public static readonly Error NotValid = new(ErrorTypes.ValidationError, "First name can only contain letters, spaces and hyphens");
            public static readonly Error TooLong = new(ErrorTypes.ValidationError, $"First name must not exceed {UserFirstName.MaxLength} characters");
        }

        /// <summary>
        /// Группа ошибок, связанных с фамилией пользователя
        /// </summary>
        public static class LastName
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, "Last name is required");
            public static readonly Error NotValid = new(ErrorTypes.ValidationError, "Last name can only contain letters, spaces and hyphens");
            public static readonly Error TooLong = new(ErrorTypes.ValidationError, $"Last name must not exceed {UserLastName.MaxLength} characters");
        }

        /// <summary>
        /// Группа ошибок, связанных с отчеством пользователя
        /// </summary>
        public static class MiddleName
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, "Middle name is required");
            public static readonly Error NotValid = new(ErrorTypes.ValidationError, "Middle name can only contain letters, spaces and hyphens");
            public static readonly Error TooLong = new(ErrorTypes.ValidationError, $"Middle name must not exceed {UserMiddleName.MaxLength} characters");
        }

        /// <summary>
        /// Группа ошибок, связанных с контактной почтой пользователя
        /// </summary>
        public static class Email
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, "Contact email is required");
            public static readonly Error NotValid = new(ErrorTypes.ValidationError, "Contact email has invalid format");
            public static readonly Error TooLong = new(ErrorTypes.ValidationError, $"Contact email must not exceed {UserEmail.MaxLength} characters");
        }

        /// <summary>
        /// Группа ошибок, связанных с контактным телефоном пользователя
        /// </summary>
        public static class PhoneNumber
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, "Phone number is required");
            public static readonly Error NotValid = new(ErrorTypes.ValidationError, "Phone number has invalid format");
            public static readonly Error TooLong = new(ErrorTypes.ValidationError, $"Phone number must not exceed {UserPhoneNumber.MaxLength} digits");
        }
    }
}
