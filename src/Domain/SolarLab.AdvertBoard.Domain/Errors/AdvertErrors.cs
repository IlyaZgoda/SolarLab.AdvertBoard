using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.SharedKernel;
using AdvertPrice = SolarLab.AdvertBoard.Domain.Adverts.Price;

namespace SolarLab.AdvertBoard.Domain.Errors
{
    /// <summary>
    /// Статический класс, содержащий ошибки, связанные с операциями над объявлениями
    /// </summary>
    public static class AdvertErrors
    {
        public static readonly Error NotFound = new(ErrorTypes.NotFound, "Advert not found");
        public static readonly Error CantUpdateNonDraftAdvert = new(ErrorTypes.ValidationError, "You can update only draft adverts");
        public static readonly Error NoChanges = new(ErrorTypes.ValidationError, "You haven't made any changes");
        public static readonly Error CanOnlyDeleteDrafts = new(ErrorTypes.ValidationError ,"Can only delete adverts in draft status");
        public static readonly Error CanOnlyUnpublishPublishedAdverts = new(ErrorTypes.ValidationError ,"Can only unpublish adverts that are currently published");
        public static readonly Error CantPublishNonDraftAdvert = new(ErrorTypes.ValidationError, "You can't publish non draft advert");
        public static readonly Error CantPublishWithNoImage = new(ErrorTypes.ValidationError, "You can't publish draft without any image");
        public static readonly Error CantArchiveNonPublishedAdvert = new(ErrorTypes.ValidationError, "You can't archive non published advert");

        /// <summary>
        /// Группа ошибок, связанных с заголовком объявления
        /// </summary>
        public static class Title
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, "Advert title is required");
            public static readonly Error TooLong = new(ErrorTypes.ValidationError, $"Advert title must not exceed {AdvertTitle.MaxLength} characters");
            public static readonly Error TooShort = new(ErrorTypes.ValidationError, $"Advert title must exceed {AdvertTitle.MinLength} characters");
        }

        /// <summary>
        /// Группа ошибок, связанных с описанием объявления
        /// </summary>
        public static class Description
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, "Advert description is required");
            public static readonly Error TooLong = new(ErrorTypes.ValidationError, $"Advert description must not exceed {AdvertDescription.MaxLength} characters");
            public static readonly Error TooShort = new(ErrorTypes.ValidationError, $"Advert description must exceed {AdvertDescription.MinLength} characters");
        }

        /// <summary>
        /// Группа ошибок, связанных с ценой объявления
        /// </summary>
        public static class Price
        {
            public static readonly Error TooLow = new(ErrorTypes.ValidationError, $"Price must be greater than {AdvertPrice.MinValue}");
            public static readonly Error TooHigh = new(ErrorTypes.ValidationError, $"Price must be less than {AdvertPrice.MinValue}");
        }
    }
}
