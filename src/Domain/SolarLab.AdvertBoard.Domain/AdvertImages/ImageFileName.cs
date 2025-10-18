using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;

namespace SolarLab.AdvertBoard.Domain.AdvertImages
{
    public record ImageFileName : IValueObject
    {
        public string Value { get; init; }

        private ImageFileName(string value) =>
            Value = value;

        private static readonly HashSet<string> AllowedExtensions = [".jpg", ".jpeg", ".png"];

        public static Result<ImageFileName> Create(string fileName) =>
            Result.Create(fileName, AdvertImageErrors.FileName.Empty)
                .Ensure(Validation.IsNotNullOrEmpty, AdvertImageErrors.FileName.Empty)
                .Map(v => Path.GetExtension(fileName).ToLowerInvariant())
                .Ensure(v => AllowedExtensions.Contains(v.ToLowerInvariant()), AdvertImageErrors.FileName.Invalid)
                .Map(v => $"{Guid.NewGuid()}{v.ToLowerInvariant()}")
                .Map(v => new ImageFileName(v));

        public static explicit operator string(ImageFileName imageFileName) => imageFileName.Value;
    }
}
