using SolarLab.AdvertBoard.Domain.Comments;
using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Domain.Errors
{
    /// <summary>
    /// Статический класс, содержащий ошибки, связанные с операциями над комментариями.
    /// </summary>
    public static class CommentErrors
    {
        public static readonly Error NotFound = new(ErrorTypes.NotFound, "Comment not found");

        /// <summary>
        /// Группа ошибок, связанных с текстом комментария.
        /// </summary>
        public static class Text
        {
            public static readonly Error Empty = new(ErrorTypes.ValidationError, "Comment text is required");

            public static readonly Error TooLong = new(ErrorTypes.ValidationError, $"Comment text must not exceed {CommentText.MaxLength} characters");
        }
    }
}
