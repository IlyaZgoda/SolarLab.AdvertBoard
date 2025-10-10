using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;

namespace SolarLab.AdvertBoard.Domain.Comments
{
    public record CommentText : IValueObject
    {
        public const int MaxLength = 1500;
        public string Value { get; init; }

        private CommentText(string value) =>
            Value = value;

        public static Result<CommentText> Create(string value) =>
            Result.Create(value, CommentErrors.Text.Empty)
                .Ensure(Validation.IsNotNullOrEmpty, CommentErrors.Text.Empty)
                .Ensure(Validation.SmallerThan(MaxLength), CommentErrors.Text.TooLong)
                .Map(v => new CommentText(v));

        public static explicit operator string(CommentText text) => text.Value;
    }
}
