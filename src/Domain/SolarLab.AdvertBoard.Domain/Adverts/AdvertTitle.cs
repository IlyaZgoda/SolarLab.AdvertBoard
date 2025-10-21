using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;

namespace SolarLab.AdvertBoard.Domain.Adverts
{
    /// <summary>
    /// Заголовок объявления как объект-значение.
    /// </summary>
    /// <param name="Value">Текст заголовка.</param>
    public record AdvertTitle : IValueObject
    {
        /// <summary>
        /// Максимальная длина заголовка.
        /// </summary>
        public const int MaxLength = 50;

        /// <summary>
        /// Минимальная длина заголовка.
        /// </summary>
        public const int MinLength = 3;

        /// <summary>
        /// Текст заголовка объявления.
        /// </summary>
        public string Value { get; init; }

        /// <summary>
        /// Приватный конструктор для создания валидного заголовка.
        /// </summary>
        /// <param name="value">Текст заголовка.</param>
        private AdvertTitle(string value) =>
            Value = value;

        /// <summary>
        /// Создает заголовок объявления с валидацией.
        /// </summary>
        /// <param name="value">Текст заголовка.</param>
        /// <returns>
        /// Успешный результат с объектом <see cref="AdvertTitle"/> или ошибку:
        /// - <see cref="AdvertErrors.Title.Empty"/> если заголовок пустой
        /// - <see cref="AdvertErrors.Title.TooShort"/> если заголовок слишком короткий
        /// - <see cref="AdvertErrors.Title.TooLong"/> если заголовок слишком длинный
        /// </returns>
        public static Result<AdvertTitle> Create(string value) =>
            Result.Create(value, AdvertErrors.Title.Empty)
                .Ensure(Validation.IsNotNullOrEmpty, AdvertErrors.Title.Empty)
                .Ensure(Validation.BiggerThan(MinLength), AdvertErrors.Title.TooShort)
                .Ensure(Validation.SmallerThan(MaxLength), AdvertErrors.Title.TooLong)
                .Map(v => new AdvertTitle(v));

        /// <summary>
        /// Явное преобразование в строку.
        /// </summary>
        /// <param name="advertTitle">Объект заголовка.</param>
        /// <returns>Текст заголовка.</returns>
        public static explicit operator string(AdvertTitle advertTitle) => advertTitle.Value;
    }
}
