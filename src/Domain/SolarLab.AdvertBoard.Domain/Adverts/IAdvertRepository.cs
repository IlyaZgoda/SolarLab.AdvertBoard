using SolarLab.AdvertBoard.SharedKernel.Maybe;
using SolarLab.AdvertBoard.SharedKernel.Specification;

namespace SolarLab.AdvertBoard.Domain.Adverts
{
    public interface IAdvertRepository
    {
        Task<Maybe<Advert>> GetByIdAsync(AdvertId id);
        Task<Maybe<Advert>> GetBySpecificationAsync(Specification<Advert> specification);
        void Add(Advert advert);
        void Update(Advert advert);
        void Delete(Advert advert);
    }
}
