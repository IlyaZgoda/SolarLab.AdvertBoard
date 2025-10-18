using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;

namespace SolarLab.AdvertBoard.Domain.AdvertImages
{
    public record ImageContentType : IValueObject
    {
        public string Value { get; init; }

        private ImageContentType(string value) =>
            Value = value;

        private static readonly HashSet<string> AllowedTypes = new(StringComparer.OrdinalIgnoreCase)
        {   
            "image/jpeg",
            "image/png",
        };

        public static Result<ImageContentType> Create(string value) =>
            Result.Create(value, AdvertImageErrors.ContentType.Empty)
                .Ensure(Validation.IsNotNullOrEmpty, AdvertImageErrors.ContentType.Empty)
                .Ensure(v => AllowedTypes.Contains(v), AdvertImageErrors.ContentType.Invalid)
                .Map(v => new ImageContentType(v));

        public static explicit operator string(ImageContentType imageContentType) => imageContentType.Value;
    }
}
