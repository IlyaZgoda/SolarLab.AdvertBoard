
using SolarLab.AdvertBoard.Application.Abstractions;
using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Application.Users.Specifications;
using SolarLab.AdvertBoard.Contracts.Comments;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Comments;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Comments.Create
{
    public class CreateCommentCommandHandler(
        IUserIdentifierProvider userIdentifierProvider, 
        IUserRepository userRepository,
        IAdvertRepository advertRepository,
        ICommentRepository commentRepository,
        IUnitOfWork unitOfWork) : ICommandHandler<CreateCommentCommand, CommentIdResponse>
    {
        public async Task<Result<CommentIdResponse>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var identityId = userIdentifierProvider.IdentityUserId;

            var user = await userRepository.GetBySpecificationAsync(new UserWithIdentitySpecification(identityId));

            var advert = await advertRepository.GetByIdAsync(new AdvertId(request.AdvertId));

            if (advert.HasNoValue)
            {
                return Result.Failure<CommentIdResponse>(AdvertErrors.NotFound);
            }

            var textResult = CommentText.Create(request.Text);

            if (textResult.IsFailure)
            {
                return Result.Failure<CommentIdResponse>(textResult.Error);
            }

            var comment = Comment.Create(advert.Value.Id, user.Value.Id, textResult.Value);

            commentRepository.Add(comment);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new CommentIdResponse(comment.Id);
        }
    }
}
