using FluentValidation;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Comments;
using SolarLab.AdvertBoard.Domain.Errors;

namespace SolarLab.AdvertBoard.Application.Extensions
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, string?> ApplyAdvertTitleValidation<T>(
            this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage(AdvertErrors.Title.Empty.Description)
                .MaximumLength(AdvertTitle.MaxLength).WithMessage(AdvertErrors.Title.TooLong.Description)
                .MinimumLength(AdvertTitle.MinLength).WithMessage(AdvertErrors.Title.TooShort.Description);
        }

        public static IRuleBuilderOptions<T, string?> ApplyAdvertDescriptionValidation<T>(
            this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage(AdvertErrors.Description.Empty.Description)
                .MaximumLength(AdvertDescription.MaxLength).WithMessage(AdvertErrors.Description.TooLong.Description)
                .MinimumLength(AdvertDescription.MinLength).WithMessage(AdvertErrors.Description.TooShort.Description);
        }

        public static IRuleBuilderOptions<T, decimal?> ApplyPriceValidation<T>(
            this IRuleBuilder<T, decimal?> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(Price.MinValue).WithMessage(AdvertErrors.Price.TooLow.Description)
                .LessThan(Price.MaxValue).WithMessage(AdvertErrors.Price.TooHigh.Description);
        }

        public static IRuleBuilderOptions<T, decimal> ApplyPriceValidation<T>(
            this IRuleBuilder<T, decimal> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(Price.MinValue).WithMessage(AdvertErrors.Price.TooLow.Description)
                .LessThan(Price.MaxValue).WithMessage(AdvertErrors.Price.TooHigh.Description);
        }

        public static IRuleBuilderOptions<T, string?> ApplyCommentTextValidation<T>(
            this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage(CommentErrors.Text.Empty.Description)
                .MaximumLength(CommentText.MaxLength).WithMessage(CommentErrors.Text.TooLong.Description); 
        }

        public static IRuleBuilderOptions<T, string> ApplyEmailValidation<T>(
            this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(320).WithMessage("Email must not exceed 320 characters");
        }

        public static IRuleBuilderOptions<T, string> ApplyPasswordValidation<T>(
            this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters")
                .MaximumLength(100).WithMessage("Password must not exceed 100 characters")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches(@"\d").WithMessage("Password must contain at least one digit")
                .Matches(@"[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]").WithMessage("Password must contain at least one special character");
        }
    }
}
