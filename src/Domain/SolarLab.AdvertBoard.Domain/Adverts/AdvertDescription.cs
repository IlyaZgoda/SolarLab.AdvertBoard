using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;

namespace SolarLab.AdvertBoard.Domain.Adverts
{
    /// <summary>
    /// Описание объявления как объект-значение.
    /// </summary>
    /// <param name="Value">Текст описания.</param>
    public record AdvertDescription : IValueObject
    {
        /// <summary>
        /// Максимальная длина описания.
        /// </summary>
        public const int MaxLength = 2000;

        /// <summary>
        /// Минимальная длина описания.
        /// </summary>
        public const int MinLength = 5;

        /// <summary>
        /// Текст описания объявления.
        /// </summary>
        public string Value { get; init; }

        /// <summary>
        /// Приватный конструктор для создания валидного описания.
        /// </summary>
        /// <param name="value">Текст описания.</param>
        private AdvertDescription(string value) =>
            Value = value;

        /// <summary>
        /// Создает описание объявления с валидацией.
        /// </summary>
        /// <param name="value">Текст описания.</param>
        /// <returns>
        /// Успешный результат с объектом <see cref="AdvertDescription"/> или ошибку:
        /// - <see cref="AdvertErrors.Description.Empty"/> если описание пустое
        /// - <see cref="AdvertErrors.Description.TooShort"/> если описание слишком короткое
        /// - <see cref="AdvertErrors.Description.TooLong"/> если описание слишком длинное
        /// </returns>
        public static Result<AdvertDescription> Create(string value) =>
            Result.Create(value, AdvertErrors.Description.Empty)
                .Ensure(Validation.IsNotNullOrEmpty, AdvertErrors.Description.Empty)
                .Ensure(Validation.BiggerThan(MinLength), AdvertErrors.Description.TooShort)
                .Ensure(Validation.SmallerThan(MaxLength), AdvertErrors.Description.TooLong)
                .Map(v => new AdvertDescription(v));

        /// <summary>
        /// Явное преобразование в строку.
        /// </summary>
        /// <param name="advertDescription">Объект описания.</param>
        /// <returns>Текст описания.</returns>
        public static explicit operator string(AdvertDescription advertDescription) => advertDescription.Value;
    }
}
