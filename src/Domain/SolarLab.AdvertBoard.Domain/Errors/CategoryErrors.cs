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

    public static class AdvertErrors
    {
        public static readonly Error NotFound = new(ErrorTypes.NotFound, "Advert not found");
        public static readonly Error CantUpdateNonDraftAdvert = new(ErrorTypes.ValidationError, "Yot can update only draft adverts");
        public static readonly Error NoChanges = new(ErrorTypes.ValidationError, "Yot haven't made any changes");
        public static readonly Error CantDeleteNonDraftAdvert = new(ErrorTypes.ValidationError, "Yot can't delete non draft advert");
        public static readonly Error CantPublishNonDraftAdvert = new(ErrorTypes.ValidationError, "Yot can't publish non draft advert");

        public static class Title
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, "Advert title is empty");
            public static readonly Error TooLong = new(ErrorTypes.ValidationError, "Advert title too long");
            public static readonly Error TooShort = new(ErrorTypes.ValidationError, "Advert title too short");
        }
        public static class Description
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, "Advert description is empty");
            public static readonly Error TooLong = new(ErrorTypes.ValidationError, "Advert description too long");
            public static readonly Error TooShort = new(ErrorTypes.ValidationError, "Advert description too short");
        }
        public static class Price
        {
            public static readonly Error TooLow = new(ErrorTypes.ValidationError, "Price too low");
            public static readonly Error TooHigh = new(ErrorTypes.ValidationError, "Price too hight");
        }
    }
}
