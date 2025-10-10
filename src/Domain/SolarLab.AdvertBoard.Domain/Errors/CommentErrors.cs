using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Domain.Errors
{
    public static class CommentErrors
    {
        public static readonly Error NotFound = new(ErrorTypes.NotFound, "Comment not found");

        public static class Text
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, "Comment is empty");
            public static readonly Error TooLong = new(ErrorTypes.ValidationError, "Comment too long");
            public static readonly Error TooShort = new(ErrorTypes.ValidationError, "Comment too short");
        }
        public static class Rating
        {
            public static readonly Error OutOfRange = new(ErrorTypes.ValidationError, "Rating must be between 1 and 5");
        }
    }
}
