using SolarLab.AdvertBoard.SharedKernel.Maybe;

namespace SolarLab.AdvertBoard.Domain.Adverts
{
    public interface IAdvertRepository
    {
        Task<Maybe<Advert>> GetByIdAsync(AdvertId id);
        void Add(Advert advert);
        void Update(Advert advert);
        void Delete(Advert advert);
    }
}
