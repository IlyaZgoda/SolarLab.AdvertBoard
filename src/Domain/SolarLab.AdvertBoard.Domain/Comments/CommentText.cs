using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;

namespace SolarLab.AdvertBoard.Domain.Comments
{
    /// <summary>
    /// Текст комментария как объект-значение.
    /// </summary>
    public record CommentText : IValueObject
    {
        /// <summary>
        /// Максимальная длина текста комментария.
        /// </summary>
        public const int MaxLength = 1500;

        /// <summary>
        /// Текст комментария.
        /// </summary>
        public string Value { get; init; }

        /// <summary>
        /// Приватный конструктор для создания валидного текста.
        /// </summary>
        /// <param name="value">Текст комментария.</param>
        private CommentText(string value) =>
            Value = value;

        /// <summary>
        /// Создает текст комментария с валидацией.
        /// </summary>
        /// <param name="value">Текст комментария.</param>
        /// <returns>
        /// Успешный результат с объектом <see cref="CommentText"/> или ошибку:
        /// - <see cref="CommentErrors.Text.Empty"/> если текст пустой
        /// - <see cref="CommentErrors.Text.TooLong"/> если текст превышает максимальную длину
        /// </returns>
        public static Result<CommentText> Create(string value) =>
            Result.Create(value, CommentErrors.Text.Empty)
                .Ensure(Validation.IsNotNullOrEmpty, CommentErrors.Text.Empty)
                .Ensure(Validation.SmallerThan(MaxLength), CommentErrors.Text.TooLong)
                .Map(v => new CommentText(v));

        /// <summary>
        /// Явное преобразование в строку.
        /// </summary>
        /// <param name="text">Объект текста комментария.</param>
        /// <returns>Текст комментария.</returns>
        public static explicit operator string(CommentText text) => text.Value;
    }
}
