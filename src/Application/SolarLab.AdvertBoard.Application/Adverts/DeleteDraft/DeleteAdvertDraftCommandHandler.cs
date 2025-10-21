using SolarLab.AdvertBoard.Application.Abstractions;
using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Adverts.DeleteDraft
{
    /// <summary>
    /// Обработчик команды <see cref="DeleteAdvertDraftCommand"/>.
    /// </summary>
    /// <param name="advertRepository">Репозиторий для работы с объявлениями.</param>
    /// <param name="unitOfWork">Unit of work.</param>
    /// <param name="userIdentifierProvider">Провайдер для получения идентификатора текущего аутентифицированного пользователя.</param>
    /// <param name="userRepository">Репозиторий для работы с пользователями.</param>
    public class DeleteAdvertDraftCommandHandler(
        IAdvertRepository advertRepository, 
        IUnitOfWork unitOfWork, 
        IUserIdentifierProvider userIdentifierProvider, 
        IUserRepository userRepository) : ICommandHandler<DeleteAdvertDraftCommand>
    {
        /// <inheritdoc/>
        public async Task<Result> Handle(DeleteAdvertDraftCommand request, CancellationToken cancellationToken)
        {
            var advert = await advertRepository.GetByIdAsync(new AdvertId(request.Id));

            if (advert.HasNoValue)
            {
                return Result.Failure(AdvertErrors.NotFound);
            }

            if (!await userRepository.IsOwner(new UserId(advert.Value.AuthorId), userIdentifierProvider.IdentityUserId))
            {
                return Result.Failure(AdvertErrors.NotFound);
            }

            advert.Value.DeleteDraft();

            advertRepository.Delete(advert.Value);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
