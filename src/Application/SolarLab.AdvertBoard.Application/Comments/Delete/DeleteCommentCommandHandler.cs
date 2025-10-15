using SolarLab.AdvertBoard.Application.Abstractions;
using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Domain.Comments;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Comments.Delete
{
    public class DeleteCommentCommandHandler(
        ICommentRepository commentRepository, 
        IUnitOfWork unitOfWork, 
        IUserIdentifierProvider userIdentifierProvider, 
        IUserRepository userRepository) : ICommandHandler<DeleteCommentCommand>
    {
        public async Task<Result> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await commentRepository.GetByIdAsync(new CommentId(request.Id));

            if (comment.HasNoValue)
            {
                return Result.Failure(CommentErrors.NotFound);
            }

            if (!await userRepository.IsOwner(comment.Value.AuthorId, userIdentifierProvider.IdentityUserId))
            {
                return Result.Failure(CommentErrors.NotFound);
            }

            commentRepository.Delete(comment.Value);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
