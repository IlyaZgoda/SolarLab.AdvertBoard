using FluentValidation;
using SolarLab.AdvertBoard.Application.Extensions;
using SolarLab.AdvertBoard.Contracts.Comments;

namespace SolarLab.AdvertBoard.Application.Comments.Create
{
    /// <summary>
    /// Валидатор запроса на создание комментария.
    /// </summary>
    public class CreateCommentRequestValidator : AbstractValidator<CreateCommentRequest>
    {
        /// <summary>
        /// Инициализирует правила валидации.
        /// </summary>
        public CreateCommentRequestValidator()
        {
            RuleFor(x => x.Text)
                .ApplyCommentTextValidation();
        }
    }
}
