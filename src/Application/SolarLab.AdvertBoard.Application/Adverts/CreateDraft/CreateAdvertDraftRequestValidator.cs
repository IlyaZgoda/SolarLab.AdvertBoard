using FluentValidation;
using SolarLab.AdvertBoard.Application.Extensions;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Errors;

namespace SolarLab.AdvertBoard.Application.Adverts.CreateDraft
{
    public class CreateAdvertDraftRequestValidator : AbstractValidator<CreateAdvertDraftRequest>
    {
        public CreateAdvertDraftRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage(AdvertErrors.Title.Empty.Description)
                .MaximumLength(AdvertTitle.MaxLength).WithMessage(AdvertErrors.Title.TooLong.Description)
                .MinimumLength(AdvertTitle.MinLength).WithMessage(AdvertErrors.Title.TooShort.Description);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(AdvertErrors.Description.Empty.Description)
                .MaximumLength(AdvertDescription.MaxLength).WithMessage(AdvertErrors.Description.TooLong.Description)
                .MinimumLength(AdvertDescription.MinLength).WithMessage(AdvertErrors.Description.TooShort.Description);

            RuleFor(x => x.Price)
                .GreaterThan(Price.MinValue).WithMessage(AdvertErrors.Price.TooLow.Description)
                .LessThan(Price.MaxValue).WithMessage(AdvertErrors.Price.TooHigh.Description);
        }
    } 
}
