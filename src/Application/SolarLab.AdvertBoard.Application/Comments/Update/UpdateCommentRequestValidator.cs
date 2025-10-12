using FluentValidation;
using SolarLab.AdvertBoard.Application.Extensions;
using SolarLab.AdvertBoard.Contracts.Comments;

namespace SolarLab.AdvertBoard.Application.Comments.Update
{
    public class UpdateCommentRequestValidator : AbstractValidator<UpdateCommentRequest>
    {
        public UpdateCommentRequestValidator()
        {
            RuleFor(x => x.Text)
                .ApplyCommentTextValidation();
        }
    }
}
