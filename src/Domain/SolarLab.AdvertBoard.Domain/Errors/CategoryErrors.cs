using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Domain.Errors
{
    /// <summary>
    /// Статический класс, содержащий ошибки, связанные с операциями над категориями.
    /// </summary>
    public static class CategoryErrors
    {
        public static readonly Error NotFound = new(ErrorTypes.NotFound, "Category not found");

        public static readonly Error CantHostAdverts = new(ErrorTypes.UnprocessableEntity, "Non leaf category can't host adverts");

        /// <summary>
        /// Группа ошибок, связанных с названием категории.
        /// </summary>
        public static class Title
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, "Category title is empty");

            public static readonly Error TooLong = new(ErrorTypes.ValidationError, "Category title too long");

            public static readonly Error TooShort = new(ErrorTypes.ValidationError, "Category title too short");
        }
    }
}
