using FluentValidation;
using SolarLab.AdvertBoard.Domain.Comments;
using SolarLab.AdvertBoard.Domain.Errors;

namespace SolarLab.AdvertBoard.Application.Extensions
{
    /// <summary>
    /// Методы расширения для FluentValidation, предоставляющие готовые правила валидации.
    /// </summary>
    public static class ValidationExtensions
    {
        /// <summary>
        /// Применяет стандартные правила валидации для текста комментария.
        /// </summary>
        /// <typeparam name="T">Тип модели, для которой применяется валидация.</typeparam>
        /// <param name="ruleBuilder">Построитель правила валидации.</param>
        /// <returns>Настроенное правило валидации.</returns>
        public static IRuleBuilderOptions<T, string?> ApplyCommentTextValidation<T>(
            this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage(CommentErrors.Text.Empty.Description)
                .MaximumLength(CommentText.MaxLength).WithMessage(CommentErrors.Text.TooLong.Description);
        }
    }
}
