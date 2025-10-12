using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.SharedKernel;
using AdvertPrice = SolarLab.AdvertBoard.Domain.Adverts.Price;

namespace SolarLab.AdvertBoard.Domain.Errors
{
    public static class AdvertErrors
    {
        public static readonly Error NotFound = new(ErrorTypes.NotFound, "Advert not found");
        public static readonly Error CantUpdateNonDraftAdvert = new(ErrorTypes.ValidationError, "Yot can update only draft adverts");
        public static readonly Error NoChanges = new(ErrorTypes.ValidationError, "Yot haven't made any changes");
        public static readonly Error CantDeleteNonDraftAdvert = new(ErrorTypes.ValidationError, "Yot can't delete non draft advert");
        public static readonly Error CantPublishNonDraftAdvert = new(ErrorTypes.ValidationError, "Yot can't publish non draft advert");
        public static readonly Error CantArchiveNonPublishedAdvert = new(ErrorTypes.ValidationError, "Yot can't archive non published advert");

        public static class Title
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, "Advert title is required");
            public static readonly Error TooLong = new(ErrorTypes.ValidationError, $"Advert title must not exceed {AdvertTitle.MaxLength} characters");
            public static readonly Error TooShort = new(ErrorTypes.ValidationError, $"Advert title must exceed {AdvertTitle.MinLength} characters");
        }
        public static class Description
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, "Advert description is required");
            public static readonly Error TooLong = new(ErrorTypes.ValidationError, $"Advert description must not exceed {AdvertDescription.MaxLength} characters");
            public static readonly Error TooShort = new(ErrorTypes.ValidationError, $"Advert description must exceed {AdvertDescription.MinLength} characters");
        }
        public static class Price
        {
            public static readonly Error TooLow = new(ErrorTypes.ValidationError, $"Price must be greater than {AdvertPrice.MinValue}");
            public static readonly Error TooHigh = new(ErrorTypes.ValidationError, $"Price must be less than {AdvertPrice.MinValue}");
        }
    }
}
