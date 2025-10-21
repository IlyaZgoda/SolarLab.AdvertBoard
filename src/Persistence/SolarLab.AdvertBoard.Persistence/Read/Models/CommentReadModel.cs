using SolarLab.AdvertBoard.Application.Abstractions.Read.Models;

namespace SolarLab.AdvertBoard.Persistence.Read.Models
{
    /// <summary>
    /// Read модель для комментария.
    /// </summary>
    public class CommentReadModel : ICommentReadModel
    {
        /// <inheritdoc/>
        public Guid Id { get; }

        /// <inheritdoc/>
        public Guid AdvertId { get; }

        public AdvertReadModel Advert { get; } = null!;

        /// <inheritdoc/>
        public Guid AuthorId { get; }

        public UserReadModel Author { get; } = null!;

        /// <inheritdoc/>
        public string Text { get; } = null!;

        /// <inheritdoc/>
        public DateTime CreatedAt { get; }

        /// <inheritdoc/>
        public DateTime? UpdatedAt { get; }

        /// <inheritdoc/>
        IAdvertReadModel ICommentReadModel.Advert => Advert;

        /// <inheritdoc/>
        IUserReadModel ICommentReadModel.Author => Author;
    }

}
