using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Application.Abstractions.Read.Providers;
using SolarLab.AdvertBoard.Application.Adverts.Specifications;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Contracts.Base;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Specification;

namespace SolarLab.AdvertBoard.Application.Adverts.GetUserDrafts
{
    /// <summary>
    /// Обработчик запроса <see cref="GetUserAdvertDraftsQuery"/>.
    /// </summary>
    /// <param name="advertReadProvider">Провайдер для чтения данных объявлений.</param>
    /// <param name="userIdentifierProvider">Провайдер для получения идентификатора текущего аутентифицированного пользователя.</param>
    public class GetUserAdvertDraftsQueryHandler(
        IAdvertReadProvider advertReadProvider, 
        IUserIdentifierProvider userIdentifierProvider) : IQueryHandler<GetUserAdvertDraftsQuery, PaginationCollection<AdvertDraftItem>>
    {
        /// <inheritdoc/>
        public async Task<Result<PaginationCollection<AdvertDraftItem>>> Handle(GetUserAdvertDraftsQuery request, CancellationToken cancellationToken)
        {
            var byUserIdentity = new AdvertByUserIdentitySpec(userIdentifierProvider.IdentityUserId);
            var drafts = new AdvertDraftSpec();
            var spec = byUserIdentity.And(drafts);

            return await advertReadProvider.GetUserAdvertDrafts(request.Page, request.PageSize, spec);
        }
            
    }
}
