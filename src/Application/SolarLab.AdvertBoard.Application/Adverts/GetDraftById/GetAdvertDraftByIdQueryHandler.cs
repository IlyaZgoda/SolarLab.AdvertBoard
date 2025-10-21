using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Application.Abstractions.Read.Providers;
using SolarLab.AdvertBoard.Application.Adverts.Specifications;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Specification;

namespace SolarLab.AdvertBoard.Application.Adverts.GetDraftById
{
    /// <summary>
    /// Обработчик запроса <see cref="GetAdvertDraftByIdQuery"/>.
    /// </summary>
    /// <param name="advertReadProvider">Провайдер для чтения данных объявлений.</param>
    /// <param name="userIdentifierProvider">Провайдер для получения идентификатора текущего аутентифицированного пользователя.</param>
    /// <param name="userRepository">Репозиторий для работы с пользователями.</param>
    public class GetAdvertByIdQueryHandler(
       IAdvertReadProvider advertReadProvider,
       IUserIdentifierProvider userIdentifierProvider,
       IUserRepository userRepository) : IQueryHandler<GetAdvertDraftByIdQuery, AdvertDraftDetailsResponse>
    {
        /// <inheritdoc/>
        public async Task<Result<AdvertDraftDetailsResponse>> Handle(GetAdvertDraftByIdQuery request, CancellationToken cancellationToken)
        {
            var byId = new AdvertWithIdSpec(request.Id);
            var draft = new AdvertDraftSpec();
            var spec = byId.And(draft);

            var response = await advertReadProvider.GetAdvertDraftDetailsByIdAsync(spec);

            if (response.HasNoValue)
            {
                return Result.Failure<AdvertDraftDetailsResponse>(AdvertErrors.NotFound);
            }

            if (!await userRepository.IsOwner(new UserId(response.Value.AuthorId), userIdentifierProvider.IdentityUserId))
            {
                return Result.Failure<AdvertDraftDetailsResponse>(AdvertErrors.NotFound);
            }

            return response.Value;
        }
    }
}
