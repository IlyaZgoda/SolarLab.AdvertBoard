using FluentValidation;
using SolarLab.AdvertBoard.Application.Extensions;
using SolarLab.AdvertBoard.Contracts.Comments;

namespace SolarLab.AdvertBoard.Application.Comments.Create
{
    public class CreateCommentRequestValidator : AbstractValidator<CreateCommentRequest>
    {
        public CreateCommentRequestValidator()
        {
            RuleFor(x => x.Text)
                .ApplyCommentTextValidation();
        }
    }
}
