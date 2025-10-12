using SolarLab.AdvertBoard.Domain.Comments;
using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Domain.Errors
{
    public static class CommentErrors
    {
        public static readonly Error NotFound = new(ErrorTypes.NotFound, "Comment not found");

        public static class Text
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, "Comment text is required");
            public static readonly Error TooLong = new(ErrorTypes.ValidationError, $"Comment text must not exceed {CommentText.MaxLength} characters");
        }
        public static class Rating
        {
            public static readonly Error OutOfRange = new(ErrorTypes.ValidationError, "Rating must be between 1 and 5");
        }
    }
}
