using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;

namespace SolarLab.AdvertBoard.Domain.Adverts
{
    public record AdvertTitle : IValueObject
    {
        public const int MaxLength = 50;
        public const int MinLength = 3;
        public string Value { get; init; }

        private AdvertTitle(string value) =>
            Value = value;

        public static Result<AdvertTitle> Create(string value) =>
            Result.Create(value, AdvertErrors.Title.Empty)
                .Ensure(Validation.IsNotNullOrEmpty, AdvertErrors.Title.Empty)
                .Ensure(Validation.BiggerThan(MinLength), AdvertErrors.Title.TooShort)
                .Ensure(Validation.SmallerThan(MaxLength), AdvertErrors.Title.TooLong)
                .Map(v => new AdvertTitle(v));

        public static explicit operator string(AdvertTitle advertTitle) => advertTitle.Value;
    }
}
