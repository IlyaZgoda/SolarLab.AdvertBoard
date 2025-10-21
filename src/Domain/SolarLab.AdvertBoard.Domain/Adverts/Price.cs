using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;

namespace SolarLab.AdvertBoard.Domain.Adverts
{
    /// <summary>
    /// Цена в объявлении как объект-значение.
    /// </summary>
    /// <param name="Value">Числовое значение цены.</param>
    public record Price : IValueObject
    {
        /// <summary>
        /// Числовое значение цены.
        /// </summary>
        public decimal Value { get; init; }

        /// <summary>
        /// Максимальное значение цены.
        /// </summary>
        public const decimal MaxValue = 100_000_000m;

        /// <summary>
        /// Минимальное значение цены.
        /// </summary>
        public const decimal MinValue = 1m;

        /// <summary>
        /// Приватный конструктор для создания валидной цены.
        /// </summary>
        /// <param name="value">Числовое значение цены.</param>
        private Price(decimal value) =>
            Value = value;

        /// <summary>
        /// Создает цену с валидацией.
        /// </summary>
        /// <param name="value">Числовое значение цены.</param>
        /// <returns>
        /// Успешный результат с объектом <see cref="Price"/> или ошибку:
        /// - <see cref="AdvertErrors.Price.TooLow"/> если цена ниже минимальной
        /// - <see cref="AdvertErrors.Price.TooHigh"/> если цена выше максимальной
        /// </returns>
        public static Result<Price> Create(decimal value) =>
            Result.CreateStruct(value, AdvertErrors.Price.TooLow)
                .Ensure(v => v >= MinValue, AdvertErrors.Price.TooLow)
                .Ensure(v => v <= MaxValue, AdvertErrors.Price.TooHigh)
                .Map(v => new Price(v));

        /// <summary>
        /// Явное преобразование в decimal.
        /// </summary>
        /// <param name="price">Объект цены.</param>
        /// <returns>Числовое значение цены.</returns>
        public static explicit operator decimal(Price price) => price.Value;
    }
}
