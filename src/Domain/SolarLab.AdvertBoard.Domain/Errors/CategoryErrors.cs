using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Domain.Errors
{
    public static class CategoryErrors
    {
        public static readonly Error NotFound = new(ErrorTypes.NotFound, "Category not found");
        public static readonly Error CantHostAdverts = new(ErrorTypes.UnprocessableEntity, "Non leaf category can't host adverts");

        public static class Title
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, "Category title is empty");
            public static readonly Error TooLong = new(ErrorTypes.ValidationError, "Category title too long");
            public static readonly Error TooShort = new(ErrorTypes.ValidationError, "Category title too short");
        }
    }
}
