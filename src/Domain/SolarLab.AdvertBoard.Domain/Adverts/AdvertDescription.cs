using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;

namespace SolarLab.AdvertBoard.Domain.Adverts
{
    public record AdvertDescription : IValueObject
    {
        public const int MaxLength = 2000;
        public const int MinLength = 5;
        public string Value { get; init; }

        private AdvertDescription(string value) =>
            Value = value;

        public static Result<AdvertDescription> Create(string value) =>
            Result.Create(value, AdvertErrors.Description.Empty)
                .Ensure(Validation.IsNotNullOrEmpty, AdvertErrors.Description.Empty)
                .Ensure(Validation.BiggerThan(MinLength), AdvertErrors.Description.TooShort)
                .Ensure(Validation.SmallerThan(MaxLength), AdvertErrors.Description.TooLong)
                .Map(v => new AdvertDescription(v));

        public static explicit operator string(AdvertDescription advertDescription) => advertDescription.Value;
    }
}
