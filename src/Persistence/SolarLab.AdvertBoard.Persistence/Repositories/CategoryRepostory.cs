using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Domain.Categories;
using SolarLab.AdvertBoard.SharedKernel.Maybe;

namespace SolarLab.AdvertBoard.Persistence.Repositories
{
    /// <summary>
    /// Репозиторий для работы с категориями.
    /// </summary>
    /// <param name="context">Контекст (для записи) базы данных.</param>
    public class CategoryRepostory(ApplicationDbContext context) : ICategoryRepository
    {
        /// <inheritdoc/>
        public void Add(Category category) => context.Add(category);

        /// <inheritdoc/>
        public async Task<Maybe<Category>> GetByIdAsync(CategoryId id) =>
            await context.Categories
                .AsNoTracking()
                .Include(c => c.Childrens) 
                .FirstOrDefaultAsync(c => c.Id == id);

        /// <inheritdoc/>
        public async Task<IReadOnlyList<Category>> GetAllAsync() =>
            await context.Categories.AsNoTracking().ToListAsync();  
    }
}
