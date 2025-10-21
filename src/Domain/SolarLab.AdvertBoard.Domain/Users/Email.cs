using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;
using System.Text.RegularExpressions;

namespace SolarLab.AdvertBoard.Domain.Users
{
    /// <summary>
    /// Контактный email пользователя как объект-значение.
    /// </summary>
    public partial record ContactEmail
    {
        /// <summary>
        /// Максимальная длина email адреса.
        /// </summary>
        public const int MaxLength = 320;

        /// <summary>
        /// Значение email адреса.
        /// </summary>
        public string Value { get; init; }

        private static readonly Regex _regex = EmailRegex();

        /// <summary>
        /// Приватный конструктор для создания валидного email.
        /// </summary>
        /// <param name="value">Email адрес.</param>
        private ContactEmail(string value) => Value = value;

        /// <summary>
        /// Создает контактный email с валидацией.
        /// </summary>
        /// <param name="value">Email адрес.</param>
        /// <returns>
        /// Успешный результат с объектом <see cref="ContactEmail"/> или ошибку:
        /// - <see cref="UserErrors.Email.Empty"/> если email пустой
        /// - <see cref="UserErrors.Email.TooLong"/> если email слишком длинный
        /// - <see cref="UserErrors.Email.NotValid"/> если email не соответствует формату
        /// </returns>
        public static Result<ContactEmail> Create(string value) =>
            Result.Create(value, UserErrors.Email.Empty)
                .Ensure(Validation.IsNotNullOrEmpty, UserErrors.Email.Empty)
                .Ensure(Validation.SmallerThan(MaxLength), UserErrors.Email.TooLong)
                .Ensure(Validation.IsMatchRegex(_regex), UserErrors.Email.NotValid)
                .Map(e => new ContactEmail(e));

        /// <summary>
        /// Явное преобразование в строку.
        /// </summary>
        /// <param name="email">Объект email.</param>
        /// <returns>Email адрес.</returns>
        public static explicit operator string(ContactEmail email) => email.Value;

        /// <summary>
        /// Генерирует регулярное выражение для валидации email.
        /// </summary>
        /// <returns>Регулярное выражение для проверки email.</returns>
        [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
        private static partial Regex EmailRegex();
    }
}
