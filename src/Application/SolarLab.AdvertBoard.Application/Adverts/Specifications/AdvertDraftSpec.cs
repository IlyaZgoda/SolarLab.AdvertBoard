using SolarLab.AdvertBoard.Application.Abstractions.Read.Models;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.SharedKernel.Specification;
using System.Linq.Expressions;

namespace SolarLab.AdvertBoard.Application.Adverts.Specifications
{
    /// <summary>
    /// Спецификация для фильтрации черновиков объявлений.
    /// </summary>
    /// <remarks>
    /// Используется для получения только тех объявлений, которые находятся в статусе черновика.
    /// Черновики доступны только автору для редактирования и не видны другим пользователям.
    /// </remarks>
    public class AdvertDraftSpec : Specification<IAdvertReadModel>
    {
        /// <inheritdoc />
        public override Expression<Func<IAdvertReadModel, bool>> PredicateExpression =>
            advert => advert.Status == AdvertStatus.Draft;
    }
}
