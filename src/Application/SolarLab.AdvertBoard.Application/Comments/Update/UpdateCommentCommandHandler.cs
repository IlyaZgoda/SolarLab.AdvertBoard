using SolarLab.AdvertBoard.Application.Abstractions;
using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Domain.Comments;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Comments.Update
{
    /// <summary>
    /// Обработчик команды <see cref="UpdateCommentCommand"/>.
    /// </summary>
    /// <param name="userIdentifierProvider">Провайдер для получения идентификатра текущего аутентифицированного пользователя.</param>
    /// <param name="userRepository">Репозиторий для работы с пользователями.</param>
    /// <param name="commentRepository">Репозиторий для работы с комментариями.</param>
    /// <param name="unitOfWork">Unit of work.</param>
    public class UpdateCommentCommandHandler(
        IUserIdentifierProvider userIdentifierProvider, 
        IUserRepository userRepository, 
        ICommentRepository commentRepository,
        IUnitOfWork unitOfWork) : ICommandHandler<UpdateCommentCommand>
    {
        /// <inheritdoc/>
        public async Task<Result> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var identityId = userIdentifierProvider.IdentityUserId;

            var comment = await commentRepository.GetByIdAsync(new CommentId(request.Id));

            if (comment.HasNoValue)
            {
                return Result.Failure(CommentErrors.NotFound);
            }

            if (!await userRepository.IsOwner(comment.Value.AuthorId, identityId))
            {
                return Result.Failure(CommentErrors.NotFound);
            }

            var textResult = CommentText.Create(request.Text);

            if (textResult.IsFailure)
            {
                return Result.Failure(textResult.Error);
            }

            comment.Value.Update(textResult.Value);

            commentRepository.Update(comment.Value);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
