using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;

namespace SolarLab.AdvertBoard.Domain.AdvertImages
{
    /// <summary>
    /// MIME-тип содержимого изображения как объект-значение.
    /// </summary>
    public record ImageContentType : IValueObject
    {
        /// <summary>
        /// MIME-тип содержимого.
        /// </summary>
        public string Value { get; init; }

        /// <summary>
        /// Приватный конструктор для создания валидного MIME-типа.
        /// </summary>
        /// <param name="value">MIME-тип.</param>
        private ImageContentType(string value) =>
            Value = value;

        /// <summary>
        /// Множество разрешенных MIME-типов для изображений.
        /// </summary>
        private static readonly HashSet<string> AllowedTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg",
        "image/png",
    };

        /// <summary>
        /// Создает MIME-тип содержимого с валидацией.
        /// </summary>
        /// <param name="value">MIME-тип.</param>
        /// <returns>
        /// Успешный результат с объектом <see cref="ImageContentType"/> или ошибку:
        /// - <see cref="AdvertImageErrors.ContentType.Empty"/> если тип пустой
        /// - <see cref="AdvertImageErrors.ContentType.Invalid"/> если тип не входит в разрешенные
        /// </returns>
        public static Result<ImageContentType> Create(string value) =>
            Result.Create(value, AdvertImageErrors.ContentType.Empty)
                .Ensure(Validation.IsNotNullOrEmpty, AdvertImageErrors.ContentType.Empty)
                .Ensure(v => AllowedTypes.Contains(v), AdvertImageErrors.ContentType.Invalid)
                .Map(v => new ImageContentType(v));

        /// <summary>
        /// Явное преобразование в string.
        /// </summary>
        /// <param name="imageContentType">Объект MIME-типа.</param>
        /// <returns>MIME-тип содержимого.</returns>
        public static explicit operator string(ImageContentType imageContentType) => imageContentType.Value;
    }
}
