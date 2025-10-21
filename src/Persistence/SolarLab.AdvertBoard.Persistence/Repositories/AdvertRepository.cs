using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.SharedKernel.Maybe;
using SolarLab.AdvertBoard.SharedKernel.Specification;

namespace SolarLab.AdvertBoard.Persistence.Repositories
{
    /// <summary>
    /// Репозиторий для работы с объявлениями.
    /// </summary>
    /// <param name="context"></param>
    public class AdvertRepository(ApplicationDbContext context) : IAdvertRepository
    {
        /// <inheritdoc/>
        public void Add(Advert advert) =>
            context.Add(advert);

        /// <inheritdoc/>
        public void Delete(Advert advert) =>
            context.Adverts.Remove(advert);

        /// <inheritdoc/>
        public async Task<Maybe<Advert>> GetByIdAsync(AdvertId id) =>
            await context.Adverts.Include(a => a.Images).FirstOrDefaultAsync(a => a.Id == id);

        /// <inheritdoc/>
        public async Task<Maybe<Advert>> GetBySpecificationAsync(Specification<Advert> specification) =>
            await context.Adverts.FirstOrDefaultAsync(specification);

        /// <inheritdoc/>
        public void Update(Advert advert) =>
            context.Update(advert);
    }
}
