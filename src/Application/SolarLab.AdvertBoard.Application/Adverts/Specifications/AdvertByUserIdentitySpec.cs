using SolarLab.AdvertBoard.Application.Abstractions.Read.Models;
using SolarLab.AdvertBoard.SharedKernel.Specification;
using System.Linq.Expressions;

namespace SolarLab.AdvertBoard.Application.Adverts.Specifications
{
    /// <summary>
    /// Спецификация для фильтрации объявлений по идентификатору пользователя в системе аутентификации.
    /// </summary>
    /// <param name="identityId">Идентификатор пользователя в системе аутентификации.</param>
    /// <remarks>
    /// Используется для получения объявлений, принадлежащих конкретному пользователю,
    /// на основе его IdentityId из системы аутентификации.
    /// </remarks>
    public class AdvertByUserIdentitySpec(string identityId) : Specification<IAdvertReadModel>
    {
        /// <inheritdoc />
        public override Expression<Func<IAdvertReadModel, bool>> PredicateExpression =>
            advert => advert.Author.IdentityId == identityId;
    }
}
