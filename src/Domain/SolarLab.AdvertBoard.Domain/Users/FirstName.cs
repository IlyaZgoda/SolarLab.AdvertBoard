using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;
using System.Text.RegularExpressions;

namespace SolarLab.AdvertBoard.Domain.Users
{
    /// <summary>
    /// Имя пользователя как объект-значение.
    /// </summary>
    public partial record FirstName
    {
        /// <summary>
        /// Максимальная длины имени.
        /// </summary>
        public const int MaxLength = 100;

        /// <summary>
        /// Значение имени.
        /// </summary>
        public string Value { get; init; }

        private static readonly Regex _regex = NameRegex();

        /// <summary>
        /// Приватный конструктор для создания валидного имени.
        /// </summary>
        /// <param name="value">Имя.</param>
        private FirstName(string value) =>
            Value = value;

        /// <summary>
        /// Создает имя с валидацией.
        /// </summary>
        /// <param name="value">Имя.</param>
        /// <returns>
        /// Успешный результат с объектом <see cref="FirstName"/> или ошибку:
        /// - <see cref="UserErrors.FirstName.Empty"/> если имя пустое
        /// - <see cref="UserErrors.FirstName.TooLong"/> если имя слишком длинное
        /// - <see cref="UserErrors.FirstName.NotValid"/> если имя содержит недопустимые символы
        /// </returns>
        public static Result<FirstName> Create(string value) =>
            Result.Create(value, UserErrors.FirstName.Empty)
                .Ensure(Validation.IsNotNullOrEmpty, UserErrors.FirstName.Empty)
                .Ensure(Validation.SmallerThan(MaxLength), UserErrors.FirstName.TooLong)
                .Ensure(Validation.IsMatchRegex(_regex), UserErrors.FirstName.NotValid)
                .Map(fn => new FirstName(fn));

        /// <summary>
        /// Явное преобразование в строку.
        /// </summary>
        /// <param name="firstName">Объект имени.</param>
        /// <returns>Имя.</returns>
        public static explicit operator string(FirstName firstName) => firstName.Value;

        /// <summary>
        /// Генерирует регулярное выражение для валидации имени.
        /// </summary>
        /// <returns>Регулярное выражение для проверки имени.</returns>
        [GeneratedRegex(@"^(?=.{1,100}$)[\p{L}'-]+(\s[\p{L}'-]+)*$")]
        public static partial Regex NameRegex();
    }
}
