using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.SharedKernel.Maybe;
using System.Threading.Tasks;

namespace SolarLab.AdvertBoard.Persistence.Repositories
{
    public class AdvertRepository(ApplicationDbContext context) : IAdvertRepository
    {
        public void Add(Advert advert) =>
            context.Add(advert);

        public void Delete(Advert advert) =>
            context.Adverts.Remove(advert);

        public async Task<Maybe<Advert>> GetByIdAsync(AdvertId id) =>
            await context.Adverts.FirstOrDefaultAsync(a => a.Id == id);

        public void Update(Advert advert) =>
            context.Update(advert);
    }
}
