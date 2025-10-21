using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Domain.Errors
{
    /// <summary>
    /// Статический класс, содержащий ошибки, связанные с операциями над изображениями объявления
    /// </summary>
    public static class AdvertImageErrors
    {
        public static readonly Error NotFound = new(ErrorTypes.NotFound, "Advert image not found");
        public static readonly Error CantAddImageToNonDraftAdvert = new(ErrorTypes.UnprocessableEntity, "You can upload images only to draft adverts");
        public static readonly Error CantDeleteImageFromNonDraftAdvert = new(ErrorTypes.UnprocessableEntity, "You can delete images only from draft adverts");
        public static readonly Error TooManyImages = new(ErrorTypes.UnprocessableEntity, "You can upload up to 5 images.");

        /// <summary>
        /// Группа ошибок, связанных с именем файла изображения
        /// </summary>
        public static class FileName
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, "Image extension is required");
            public static readonly Error Invalid = new(ErrorTypes.ValidationError, $"Image extension has invalid format");
            public static readonly Error TooShort = new(ErrorTypes.ValidationError, $"Image extension must exceed 1 characters");
        }

        /// <summary>
        /// Группа ошибок, связанных с MIME-типом содержимого изображения
        /// </summary>
        public static class ContentType
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, "Image content type is required");
            public static readonly Error Invalid = new(ErrorTypes.ValidationError, $"Image content type has invalid format");
        }

        /// <summary>
        /// Группа ошибок, связанных с бинарным содержимым изображения.
        /// </summary>
        public static class Content
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, $"Image content is required");
            public static readonly Error Invalid = new(ErrorTypes.ValidationError, $"Image content has invalid format");
        }
    }
}
