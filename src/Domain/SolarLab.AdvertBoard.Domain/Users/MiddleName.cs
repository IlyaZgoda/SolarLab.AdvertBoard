using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;
using System.Text.RegularExpressions;

namespace SolarLab.AdvertBoard.Domain.Users
{
    /// <summary>
    /// Отчество пользователя как объект-значение.
    /// </summary>
    public record MiddleName
    {
        /// <summary>
        /// Максимальная длина отчества.
        /// </summary>
        public const int MaxLength = 100;

        /// <summary>
        /// Значение отчества.
        /// </summary>
        public string? Value { get; init; }

        private static readonly Regex _regex = FirstName.NameRegex();

        /// <summary>
        /// Приватный конструктор для создания валидного отчества.
        /// </summary>
        /// <param name="value">Отчество.</param>
        private MiddleName(string? value) =>
            Value = value;

        /// <summary>
        /// Создает отчество с валидацией.
        /// </summary>
        /// <param name="value">Отчество.</param>
        /// <returns>
        /// Успешный результат с объектом <see cref="MiddleName"/> или ошибку:
        /// - <see cref="UserErrors.MiddleName.Empty"/> если отчество пустое (только для ненулевых значений)
        /// - <see cref="UserErrors.MiddleName.TooLong"/> если отчество слишком длинное
        /// - <see cref="UserErrors.MiddleName.NotValid"/> если отчество содержит недопустимые символы
        /// </returns>
        /// <remarks>
        /// Возвращает null если передана пустая строка или null.
        /// </remarks>
        public static Result<MiddleName?> Create(string? value) =>
            string.IsNullOrWhiteSpace(value)
            ? Result.Success<MiddleName?>(null)
            : Result.Create(value, UserErrors.MiddleName.Empty)
                .Ensure(Validation.SmallerThan(MaxLength), UserErrors.MiddleName.TooLong)
                .Ensure(Validation.IsMatchRegex(_regex), UserErrors.MiddleName.NotValid)
                .MapNullable(mn => new MiddleName(mn));

        /// <summary>
        /// Явное преобразование в строку.
        /// </summary>
        /// <param name="middleName">Объект отчества.</param>
        /// <returns>Отчество или пустая строка если null.</returns>
        public static explicit operator string(MiddleName middleName) => middleName?.Value ?? string.Empty;
    }
}
