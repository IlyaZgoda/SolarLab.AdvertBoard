using SolarLab.AdvertBoard.Application.Abstractions.Read.Models;
using SolarLab.AdvertBoard.SharedKernel.Specification;
using System.Linq.Expressions;

namespace SolarLab.AdvertBoard.Application.Adverts.Specifications
{
    /// <summary>
    /// Спецификация для фильтрации объявлений по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор объявления.</param>
    /// <remarks>
    /// Используется для получения конкретного объявления по его уникальному идентификатору.
    /// </remarks>
    public class AdvertWithIdSpec(Guid id) : Specification<IAdvertReadModel>
    {
        /// <inheritdoc />
        public override Expression<Func<IAdvertReadModel, bool>> PredicateExpression =>
            advert => advert.Id == id;
    }
}
