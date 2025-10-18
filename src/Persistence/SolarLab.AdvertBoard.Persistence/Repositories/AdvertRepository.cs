using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.SharedKernel.Maybe;
using SolarLab.AdvertBoard.SharedKernel.Specification;
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
            await context.Adverts.Include(a => a.Images).FirstOrDefaultAsync(a => a.Id == id);

        public async Task<Maybe<Advert>> GetBySpecificationAsync(Specification<Advert> specification) =>
            await context.Adverts.FirstOrDefaultAsync(specification);

        public void Update(Advert advert) =>
            context.Update(advert);
    }
}
