using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;
using System.Text.RegularExpressions;

namespace SolarLab.AdvertBoard.Domain.Users
{
    /// <summary>
    /// Фамилия пользователя как объект-значение.
    /// </summary>
    public record LastName
    {
        /// <summary>
        /// Максимальная длина фамилии.
        /// </summary>
        public const int MaxLength = 100;

        /// <summary>
        /// Значение фамилии.
        /// </summary>
        public string Value { get; init; }

        private static readonly Regex _regex = FirstName.NameRegex();

        /// <summary>
        /// Приватный конструктор для создания валидной фамилии.
        /// </summary>
        /// <param name="value">Фамилия.</param>
        private LastName(string value) =>
            Value = value;

        /// <summary>
        /// Создает фамилию с валидацией.
        /// </summary>
        /// <param name="value">Фамилия.</param>
        /// <returns>
        /// Успешный результат с объектом <see cref="LastName"/> или ошибку:
        /// - <see cref="UserErrors.LastName.Empty"/> если фамилия пустая
        /// - <see cref="UserErrors.LastName.TooLong"/> если фамилия слишком длинная
        /// - <see cref="UserErrors.LastName.NotValid"/> если фамилия содержит недопустимые символы
        /// </returns>
        public static Result<LastName> Create(string value) =>
            Result.Create(value, UserErrors.LastName.Empty)
                .Ensure(Validation.IsNotNullOrEmpty, UserErrors.LastName.Empty)
                .Ensure(Validation.SmallerThan(MaxLength), UserErrors.LastName.TooLong)
                .Ensure(Validation.IsMatchRegex(_regex), UserErrors.LastName.NotValid)
                .Map(ln => new LastName(ln));

        /// <summary>
        /// Явное преобразование в строку.
        /// </summary>
        /// <param name="lastName">Объект фамилии.</param>
        /// <returns>Фамилия.</returns>
        public static explicit operator string(LastName lastName) => lastName.Value;
    }
}
