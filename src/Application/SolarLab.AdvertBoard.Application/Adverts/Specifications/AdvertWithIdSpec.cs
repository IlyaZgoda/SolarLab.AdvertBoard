using SolarLab.AdvertBoard.Application.Abstractions.Read.Models;
using SolarLab.AdvertBoard.SharedKernel.Specification;
using System.Linq.Expressions;

namespace SolarLab.AdvertBoard.Application.Adverts.Specifications
{
    public class AdvertWithIdSpec(Guid id) : Specification<IAdvertReadModel>
    {
        public override Expression<Func<IAdvertReadModel, bool>> PredicateExpression =>
            advert => advert.Id == id;
    }
}
