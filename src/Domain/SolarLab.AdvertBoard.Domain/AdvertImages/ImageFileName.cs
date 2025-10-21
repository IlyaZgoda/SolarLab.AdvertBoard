using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;

namespace SolarLab.AdvertBoard.Domain.AdvertImages
{
    /// <summary>
    /// Имя файла изображения как объект-значение.
    /// </summary>
    public record ImageFileName : IValueObject
    {
        /// <summary>
        /// Имя файла.
        /// </summary>
        public string Value { get; init; }

        /// <summary>
        /// Приватный конструктор для создания валидного имени файла.
        /// </summary>
        /// <param name="value">Имя файла.</param>
        private ImageFileName(string value) =>
            Value = value;

        /// <summary>
        /// Множество разрешенных расширений файлов.
        /// </summary>
        private static readonly HashSet<string> AllowedExtensions = [".jpg", ".jpeg", ".png"];

        /// <summary>
        /// Создает имя файла изображения с валидацией и нормализацией.
        /// </summary>
        /// <param name="fileName">Оригинальное имя файла.</param>
        /// <returns>
        /// Успешный результат с объектом <see cref="ImageFileName"/> или ошибку:
        /// - <see cref="AdvertImageErrors.FileName.Empty"/> если имя файла пустое
        /// - <see cref="AdvertImageErrors.FileName.Invalid"/> если расширение файла не разрешено
        /// </returns>
        public static Result<ImageFileName> Create(string fileName) =>
            Result.Create(fileName, AdvertImageErrors.FileName.Empty)
                .Ensure(Validation.IsNotNullOrEmpty, AdvertImageErrors.FileName.Empty)
                .Map(v => Path.GetExtension(fileName).ToLowerInvariant())
                .Ensure(v => AllowedExtensions.Contains(v.ToLowerInvariant()), AdvertImageErrors.FileName.Invalid)
                .Map(v => $"{Guid.NewGuid()}{v.ToLowerInvariant()}")
                .Map(v => new ImageFileName(v));

        /// <summary>
        /// Явное преобразование в string.
        /// </summary>
        /// <param name="imageFileName">Объект имени файла.</param>
        /// <returns>Имя файла.</returns>
        public static explicit operator string(ImageFileName imageFileName) => imageFileName.Value;
    }
}
