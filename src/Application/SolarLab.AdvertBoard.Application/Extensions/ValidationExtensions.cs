using FluentValidation;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Comments;
using SolarLab.AdvertBoard.Domain.Errors;

namespace SolarLab.AdvertBoard.Application.Extensions
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, string?> ApplyCommentTextValidation<T>(
            this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage(CommentErrors.Text.Empty.Description)
                .MaximumLength(CommentText.MaxLength).WithMessage(CommentErrors.Text.TooLong.Description); 
        }
    }
}
