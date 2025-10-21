using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;
using System.Text.RegularExpressions;

namespace SolarLab.AdvertBoard.Domain.Users
{
    /// <summary>
    /// Номер телефона пользователя как объект-значение.
    /// </summary>
    public partial record PhoneNumber
    {
        /// <summary>
        /// Максимальная длина номера телефона.
        /// </summary>
        public const int MaxLength = 15;

        /// <summary>
        /// Значение номера телефона.
        /// </summary>
        public string? Value { get; init; }

        private static readonly Regex _regex = PhoneNumberRegex();

        /// <summary>
        /// Приватный конструктор для создания валидного номера телефона.
        /// </summary>
        /// <param name="value">Номер телефона.</param>
        private PhoneNumber(string value) => Value = value;

        /// <summary>
        /// Создает номер телефона с валидацией.
        /// </summary>
        /// <param name="value">Номер телефона.</param>
        /// <returns>
        /// Успешный результат с объектом <see cref="PhoneNumber"/> или ошибку:
        /// - <see cref="UserErrors.PhoneNumber.Empty"/> если номер пустой (только для ненулевых значений)
        /// - <see cref="UserErrors.PhoneNumber.TooLong"/> если номер слишком длинный
        /// - <see cref="UserErrors.PhoneNumber.NotValid"/> если номер не соответствует формату
        /// </returns>
        /// <remarks>
        /// Возвращает null если передана пустая строка или null.
        /// Требует формата: +79001234567 (код страны + 10-11 цифр)
        /// </remarks>
        public static Result<PhoneNumber?> Create(string? value) =>
            string.IsNullOrWhiteSpace(value)
            ? Result.Success<PhoneNumber?>(null)
            : Result.Create(value, UserErrors.PhoneNumber.Empty)
                .Ensure(Validation.IsNotNullOrEmpty, UserErrors.PhoneNumber.Empty)
                .Ensure(Validation.SmallerThan(MaxLength), UserErrors.PhoneNumber.TooLong)
                .Ensure(Validation.IsMatchRegex(_regex), UserErrors.PhoneNumber.NotValid)
                .MapNullable(pn => new PhoneNumber(pn));

        /// <summary>
        /// Явное преобразование в строку.
        /// </summary>
        /// <param name="phoneNumber">Объект номера телефона.</param>
        /// <returns>Номер телефона или пустая строка если null.</returns>
        public static explicit operator string(PhoneNumber phoneNumber) => phoneNumber?.Value ?? string.Empty;

        /// <summary>
        /// Генерирует регулярное выражение для валидации номера телефона.
        /// </summary>
        /// <returns>Регулярное выражение для проверки номера телефона.</returns>
        [GeneratedRegex(@"^\+\d{11,14}$")]
        public static partial Regex PhoneNumberRegex();
    }
}
