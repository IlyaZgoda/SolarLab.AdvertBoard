using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Domain.Errors
{
    public static class AdvertImageErrors
    {
        public static readonly Error NotFound = new(ErrorTypes.NotFound, "Advert image not found");
        public static readonly Error CantAddImageToNonDraftAdvert = new(ErrorTypes.UnprocessableEntity, "You can upload images only to draft adverts");
        public static readonly Error CantDeleteImageFromNonDraftAdvert = new(ErrorTypes.UnprocessableEntity, "You can delete images only from draft adverts");
        public static readonly Error TooManyImages = new(ErrorTypes.UnprocessableEntity, "You can upload up to 5 images.");

        public static class FileName
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, "Image extension is required");
            public static readonly Error Invalid = new(ErrorTypes.ValidationError, $"Image extension has invalid format");
            public static readonly Error TooShort = new(ErrorTypes.ValidationError, $"Image extension must exceed 1 characters");
        }
        public static class ContentType
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, "Image content type is required");
            public static readonly Error Invalid = new(ErrorTypes.ValidationError, $"Image content type has invalid format");
        }
        public static class Content
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, $"Image content is required");
            public static readonly Error Invalid = new(ErrorTypes.ValidationError, $"Image content has invalid format");
        }
    }
}
