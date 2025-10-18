using SolarLab.AdvertBoard.Domain.Adverts;

namespace SolarLab.AdvertBoard.Persistence.Read.Models
{
    public class AdvertReadModel
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public UserReadModel Author { get; set; } = null!;

        public Guid CategoryId { get; set; }
        public CategoryReadModel Category { get; set; } = null!;

        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public AdvertStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<AdvertImageReadModel> Images { get; set; } = new();
        public List<CommentReadModel> Comments { get; set; } = new();
    }

}
