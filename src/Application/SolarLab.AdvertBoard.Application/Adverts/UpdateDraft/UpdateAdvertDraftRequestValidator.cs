using FluentValidation;
using SolarLab.AdvertBoard.Application.Extensions;
using SolarLab.AdvertBoard.Contracts.Adverts;

namespace SolarLab.AdvertBoard.Application.Adverts.UpdateDraft
{
    public class UpdateAdvertDraftRequestValidator : AbstractValidator<UpdateAdvertDraftRequest>
    {
        public UpdateAdvertDraftRequestValidator()
        {
            RuleFor(x => x.Title)
                .ApplyAdvertTitleValidation()
                .When(x => !string.IsNullOrEmpty(x.Title));

            RuleFor(x => x.Description)
                .ApplyAdvertDescriptionValidation()
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.Price)
                .ApplyPriceValidation()
                .When(x => x.Price.HasValue);
        }
    }
}
