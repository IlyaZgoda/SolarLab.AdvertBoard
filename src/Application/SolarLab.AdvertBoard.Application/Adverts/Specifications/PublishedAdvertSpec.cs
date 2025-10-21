using SolarLab.AdvertBoard.Application.Abstractions.Read.Models;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.SharedKernel.Specification;
using System.Linq.Expressions;

namespace SolarLab.AdvertBoard.Application.Adverts.Specifications
{
    /// <summary>
    /// Спецификация для фильтрации опубликованных объявлений.
    /// </summary>
    /// <remarks>
    /// Используется для получения только тех объявлений, которые находятся в статусе опубликованных.
    /// Опубликованные объявления видны всем пользователям системы.
    /// </remarks>
    public class PublishedAdvertSpec : Specification<IAdvertReadModel>
    {
        /// <inheritdoc />
        public override Expression<Func<IAdvertReadModel, bool>> PredicateExpression =>
            advert => advert.Status == AdvertStatus.Published;
    }
}
