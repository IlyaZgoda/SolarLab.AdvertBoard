using SolarLab.AdvertBoard.Application.Abstractions.Read.Models;
using SolarLab.AdvertBoard.Domain.Adverts;

namespace SolarLab.AdvertBoard.Persistence.Read.Models
{
    public class AdvertReadModel : IAdvertReadModel
    {
        public Guid Id { get; }
        public Guid AuthorId { get; }
        public UserReadModel Author { get; } = null!;

        public Guid CategoryId { get; }
        public CategoryReadModel Category { get; } = null!;

        public string Title { get; } = null!;
        public string Description { get; } = null!;
        public decimal Price { get; }
        public AdvertStatus Status { get; }
        public DateTime CreatedAt { get; }
        public DateTime? PublishedAt { get; }
        public DateTime? UpdatedAt { get; }

        public List<AdvertImageReadModel> Images { get; } = [];
        public List<CommentReadModel> Comments { get; } = [];

        IUserReadModel IAdvertReadModel.Author => Author;

        ICategoryReadModel IAdvertReadModel.Category => Category;

        IReadOnlyList<IAdvertImageReadModel> IAdvertReadModel.Images => Images;

        IReadOnlyList<ICommentReadModel> IAdvertReadModel.Comments => Comments;
    }

}
