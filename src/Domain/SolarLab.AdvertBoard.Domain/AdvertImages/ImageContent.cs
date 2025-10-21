using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;

namespace SolarLab.AdvertBoard.Domain.AdvertImages
{
    /// <summary>
    /// Бинарное содержимое изображения как объект-значение.
    /// </summary>
    public record ImageContent : IValueObject
    {
        /// <summary>
        /// Бинарное содержимое изображения.
        /// </summary>
        public byte[] Value { get; init; }

        /// <summary>
        /// Приватный конструктор для создания валидного содержимого.
        /// </summary>
        /// <param name="value">Бинарное содержимое.</param>
        private ImageContent(byte[] value) =>
            Value = value;

        /// <summary>
        /// Проверяет сигнатуру файла для определения валидности изображения.
        /// </summary>
        /// <param name="data">Бинарные данные для проверки.</param>
        /// <returns>true если данные содержат валидную сигнатуру изображения, иначе false.</returns>
        private static bool HasValidSignature(byte[] data)
        {
            // JPEG сигнатура: FF D8 FF
            if (data.Length > 3 && data[0] == 0xFF && data[1] == 0xD8 && data[2] == 0xFF)
                return true;

            // PNG сигнатура: 89 50 4E 47 0D 0A 1A 0A
            if (data.Length > 7 && data[0] == 0x89 && data[1] == 0x50 &&
                data[2] == 0x4E && data[3] == 0x47 && data[4] == 0x0D &&
                data[5] == 0x0A && data[6] == 0x1A && data[7] == 0x0A)
                return true;

            return false;
        }

        /// <summary>
        /// Создает содержимое изображения с валидацией.
        /// </summary>
        /// <param name="data">Бинарные данные изображения.</param>
        /// <returns>
        /// Успешный результат с объектом <see cref="ImageContent"/> или ошибку:
        /// - <see cref="AdvertImageErrors.Content.Empty"/> если данные пустые
        /// - <see cref="AdvertImageErrors.Content.Invalid"/> если сигнатура файла невалидна
        /// </returns>
        public static Result<ImageContent> Create(byte[] data) =>
            Result.Create(data, AdvertImageErrors.Content.Empty)
                .Ensure(v => v is not null && v.Length > 0, AdvertImageErrors.Content.Empty)
                .Ensure(v => HasValidSignature(data), AdvertImageErrors.Content.Invalid)
                .Map(v => new ImageContent(v));

        /// <summary>
        /// Явное преобразование в byte[].
        /// </summary>
        /// <param name="imageContent">Объект содержимого изображения.</param>
        /// <returns>Бинарное содержимое изображения.</returns>
        public static explicit operator byte[](ImageContent imageContent) => imageContent.Value;
    }
}
