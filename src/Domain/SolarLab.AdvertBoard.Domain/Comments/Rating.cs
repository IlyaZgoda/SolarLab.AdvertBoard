using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;

namespace SolarLab.AdvertBoard.Domain.Comments
{
    public record Rating : IValueObject
    {
        public const int MaxValue = 5;
        public const int MinValue = 1;
        public int Value { get; init; }

        private Rating(int value) =>
            Value = value;

        public static Result<Rating> Create(int value) =>
            Result.CreateStruct(value, CommentErrors.Text.Empty)
                .Ensure(v => v <= MaxValue && v >= MinValue, CommentErrors.Rating.OutOfRange)
                .Map(v => new Rating(v));

        public static explicit operator int(Rating rating) => rating.Value;
    }
}
