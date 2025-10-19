using SolarLab.AdvertBoard.Application.Abstractions.Read.Models;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.SharedKernel.Specification;
using System.Linq.Expressions;

namespace SolarLab.AdvertBoard.Application.Adverts.Specifications
{
    public class PublishedAdvertSpec : Specification<IAdvertReadModel>
    {
        public override Expression<Func<IAdvertReadModel, bool>> PredicateExpression =>
            advert => advert.Status == AdvertStatus.Published;
    }
}
