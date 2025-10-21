using SolarLab.AdvertBoard.Application.Abstractions;
using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Domain.Comments;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Comments.Delete
{
    /// <summary>
    /// Обработчик команды <see cref="DeleteCommentCommand"/>.
    /// </summary>
    /// <param name="commentRepository">Репозиторий для работы с комментариями.</param>
    /// <param name="unitOfWork">Unit of work.</param>
    /// <param name="userIdentifierProvider">Провайдер для получения идентификатора текущего аутентифицированного пользователя.</param>
    /// <param name="userRepository">Репозиторий для работы с пользователями.</param>
    public class DeleteCommentCommandHandler(
        ICommentRepository commentRepository, 
        IUnitOfWork unitOfWork, 
        IUserIdentifierProvider userIdentifierProvider, 
        IUserRepository userRepository) : ICommandHandler<DeleteCommentCommand>
    {
        /// <inheritdoc/>
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
