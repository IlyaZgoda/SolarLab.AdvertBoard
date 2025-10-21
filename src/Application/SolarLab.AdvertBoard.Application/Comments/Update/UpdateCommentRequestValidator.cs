using FluentValidation;
using SolarLab.AdvertBoard.Application.Extensions;
using SolarLab.AdvertBoard.Contracts.Comments;

namespace SolarLab.AdvertBoard.Application.Comments.Update
{
    /// <summary>
    /// Валидатор запроса на обновления комментария.
    /// </summary>
    public class UpdateCommentRequestValidator : AbstractValidator<UpdateCommentRequest>
    {
        /// <summary>
        /// Инициализирует правила валидации.
        /// </summary>
        public UpdateCommentRequestValidator()
        {
            RuleFor(x => x.Text)
                .ApplyCommentTextValidation();
        }
    }
}
