using SolarLab.AdvertBoard.SharedKernel.Maybe;

namespace SolarLab.AdvertBoard.Domain.Categories
{
    public interface ICategoryRepository
    {
        void Add(Category category);
        Task<Maybe<Category>> GetByIdAsync(CategoryId id);
        Task<IReadOnlyList<Category>> GetAllAsync();
    }
}
