using SolarLab.AdvertBoard.Application.Abstractions.Read.Models;
using SolarLab.AdvertBoard.Domain.Adverts;

namespace SolarLab.AdvertBoard.Persistence.Read.Models
{
    /// <summary>
    /// Read модель для объявления.
    /// </summary>
    public class AdvertReadModel : IAdvertReadModel
    {
        /// <inheritdoc/>
        public Guid Id { get; }

        /// <inheritdoc/>
        public Guid AuthorId { get; }

        public UserReadModel Author { get; } = null!;

        /// <inheritdoc/>
        public Guid CategoryId { get; }

        public CategoryReadModel Category { get; } = null!;

        /// <inheritdoc/>
        public string Title { get; } = null!;

        /// <inheritdoc/>
        public string Description { get; } = null!;

        /// <inheritdoc/>
        public decimal Price { get; }

        /// <inheritdoc/>
        public AdvertStatus Status { get; }

        /// <inheritdoc/>
        public DateTime CreatedAt { get; }

        /// <inheritdoc/>
        public DateTime? PublishedAt { get; }

        /// <inheritdoc/>
        public DateTime? UpdatedAt { get; }

        public List<AdvertImageReadModel> Images { get; } = [];

        public List<CommentReadModel> Comments { get; } = [];

        /// <inheritdoc/>
        IUserReadModel IAdvertReadModel.Author => Author;

        /// <inheritdoc/>
        ICategoryReadModel IAdvertReadModel.Category => Category;

        /// <inheritdoc/>
        IReadOnlyList<IAdvertImageReadModel> IAdvertReadModel.Images => Images;

        /// <inheritdoc/>
        IReadOnlyList<ICommentReadModel> IAdvertReadModel.Comments => Comments;
    }

}
