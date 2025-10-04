using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Domain.Categories;
using SolarLab.AdvertBoard.SharedKernel.Maybe;
using System.Threading.Tasks;

namespace SolarLab.AdvertBoard.Persistence.Repositories
{
    public class CategoryRepostory(ApplicationDbContext context) : ICategoryRepository
    {
        public void Add(Category category) => context.Add(category);

        public async Task<Maybe<Category>> GetByIdAsync(CategoryId id) =>
            await context.Categories
                .AsNoTracking()
                .Include(c => c.Childrens) 
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<IReadOnlyList<Category>> GetAllAsync() =>
            await context.Categories.AsNoTracking().ToListAsync();  
    }
}
