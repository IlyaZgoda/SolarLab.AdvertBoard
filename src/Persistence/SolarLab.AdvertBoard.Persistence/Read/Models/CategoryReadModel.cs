using SolarLab.AdvertBoard.Application.Abstractions.Read.Models;

namespace SolarLab.AdvertBoard.Persistence.Read.Models
{
    /// <summary>
    /// Read модель для категории.
    /// </summary>
    public class CategoryReadModel : ICategoryReadModel
    {
        /// <inheritdoc/>
        public Guid Id { get; }

        /// <inheritdoc/>
        public Guid? ParentId { get; }

        /// <inheritdoc/>
        public string Title { get; } = null!;

        public List<CategoryReadModel> Childrens { get; } = [];

        /// <inheritdoc/>
        IReadOnlyList<ICategoryReadModel> ICategoryReadModel.Childrens => Childrens;
    }
}
