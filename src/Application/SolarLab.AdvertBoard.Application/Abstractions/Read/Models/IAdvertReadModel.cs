using SolarLab.AdvertBoard.Domain.Adverts;

namespace SolarLab.AdvertBoard.Application.Abstractions.Read.Models
{
    public interface IAdvertReadModel
    {
        public Guid Id { get; }
        public Guid AuthorId { get; }
        public IUserReadModel Author { get; }

        public Guid CategoryId { get; }
        public ICategoryReadModel Category { get; }

        public string Title { get; }
        public string Description { get; }
        public decimal Price { get; }
        public AdvertStatus Status { get; }
        public DateTime CreatedAt { get; }
        public DateTime? PublishedAt { get; }
        public DateTime? UpdatedAt { get; }

        public IReadOnlyList<IAdvertImageReadModel> Images { get; }
        public IReadOnlyList<ICommentReadModel> Comments { get; }
    }
}
