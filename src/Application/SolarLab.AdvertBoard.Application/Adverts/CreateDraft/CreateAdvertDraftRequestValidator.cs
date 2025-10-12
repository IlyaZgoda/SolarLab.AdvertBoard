using FluentValidation;
using SolarLab.AdvertBoard.Application.Extensions;
using SolarLab.AdvertBoard.Contracts.Adverts;

namespace SolarLab.AdvertBoard.Application.Adverts.CreateDraft
{
    public class CreateAdvertDraftRequestValidator : AbstractValidator<CreateAdvertDraftRequest>
    {
        public CreateAdvertDraftRequestValidator()
        {
            RuleFor(x => x.Title)
                .ApplyAdvertTitleValidation();

            RuleFor(x => x.Description)
                .ApplyAdvertDescriptionValidation();

            RuleFor(x => x.Price)
                .ApplyPriceValidation();
        }
    } 
}
