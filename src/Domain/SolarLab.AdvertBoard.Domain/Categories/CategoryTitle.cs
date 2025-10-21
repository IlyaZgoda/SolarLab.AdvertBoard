using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;

namespace SolarLab.AdvertBoard.Domain.Categories
{
    /// <summary>
    /// Название категории как объект-значение.
    /// </summary>
    public record CategoryTitle
    {
        /// <summary>
        /// Максимальная длина названия категории.
        /// </summary>
        public const int MaxLength = 50;

        /// <summary>
        /// Минимальная длина названия категории.
        /// </summary>
        public const int MinLength = 3;

        /// <summary>
        /// Текст названия категории.
        /// </summary>
        public string Value { get; init; }

        /// <summary>
        /// Приватный конструктор для создания валидного названия.
        /// </summary>
        /// <param name="value">Текст названия.</param>
        private CategoryTitle(string value) =>
            Value = value;

        /// <summary>
        /// Создает название категории с валидацией.
        /// </summary>
        /// <param name="value">Текст названия.</param>
        /// <returns>
        /// Успешный результат с объектом <see cref="CategoryTitle"/> или ошибку:
        /// - <see cref="CategoryErrors.Title.Empty"/> если название пустое
        /// - <see cref="CategoryErrors.Title.TooShort"/> если название слишком короткое
        /// - <see cref="CategoryErrors.Title.TooLong"/> если название слишком длинное
        /// </returns>
        public static Result<CategoryTitle> Create(string value) =>
            Result.Create(value, CategoryErrors.Title.Empty)
                .Ensure(Validation.IsNotNullOrEmpty, CategoryErrors.Title.Empty)
                .Ensure(Validation.BiggerThan(MinLength), CategoryErrors.Title.TooShort)
                .Ensure(Validation.SmallerThan(MaxLength), CategoryErrors.Title.TooLong)
                .Map(ct => new CategoryTitle(ct));

        /// <summary>
        /// Явное преобразование в строку.
        /// </summary>
        /// <param name="categoryTitle">Объект названия категории.</param>
        /// <returns>Текст названия категории.</returns>
        public static explicit operator string(CategoryTitle categoryTitle) => categoryTitle.Value;
    }
}
