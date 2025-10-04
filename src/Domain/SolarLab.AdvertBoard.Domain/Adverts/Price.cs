using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;

namespace SolarLab.AdvertBoard.Domain.Adverts
{
    public record Price : IValueObject
    {
        public decimal Value { get; init; }
        public const decimal MaxValue = 100_000_000m;
        public const decimal MinValue = 1m;

        private Price(decimal value) =>
            Value = value;

        public static Result<Price> Create(decimal value) =>
            Result.CreateStruct(value, AdvertErrors.Price.TooLow)
                .Ensure(v => v >= MinValue, AdvertErrors.Price.TooLow)
                .Ensure(v => v <= MaxValue, AdvertErrors.Price.TooHigh)
                .Map(v => new Price(v));

        public static explicit operator decimal(Price price) => price.Value;
    }
}
