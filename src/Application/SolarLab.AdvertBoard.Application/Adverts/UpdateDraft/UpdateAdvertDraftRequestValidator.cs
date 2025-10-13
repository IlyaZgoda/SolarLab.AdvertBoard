using FluentValidation;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Errors;

namespace SolarLab.AdvertBoard.Application.Adverts.UpdateDraft
{
    public class UpdateAdvertDraftRequestValidator : AbstractValidator<UpdateAdvertDraftRequest>
    {
        public UpdateAdvertDraftRequestValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(AdvertTitle.MaxLength).WithMessage(AdvertErrors.Title.TooLong.Description)
                .MinimumLength(AdvertTitle.MinLength).WithMessage(AdvertErrors.Title.TooShort.Description)
                .When(x => !string.IsNullOrWhiteSpace(x.Title));

            RuleFor(x => x.Description)
                .MaximumLength(AdvertDescription.MaxLength).WithMessage(AdvertErrors.Description.TooLong.Description)
                .MinimumLength(AdvertDescription.MinLength).WithMessage(AdvertErrors.Description.TooShort.Description)
                .When(x => !string.IsNullOrWhiteSpace(x.Description));

            RuleFor(x => x.Price)
                .GreaterThan(Price.MinValue).WithMessage(AdvertErrors.Price.TooLow.Description)
                .LessThan(Price.MaxValue).WithMessage(AdvertErrors.Price.TooHigh.Description)
                .When(x => x.Price.HasValue);
        }
    }
}
