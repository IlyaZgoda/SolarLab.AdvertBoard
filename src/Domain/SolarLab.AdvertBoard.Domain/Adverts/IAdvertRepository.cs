using SolarLab.AdvertBoard.SharedKernel.Maybe;

namespace SolarLab.AdvertBoard.Domain.Adverts
{
    public interface IAdvertRepository
    {
        Task<Maybe<Advert>> GetById(AdvertId id);
        void Add(Advert advert);
        void Update(Advert advert);
    }
}
