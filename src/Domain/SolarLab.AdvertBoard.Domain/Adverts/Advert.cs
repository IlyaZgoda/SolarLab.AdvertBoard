using SolarLab.AdvertBoard.Domain.Categories;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Domain.Adverts
{
    public class Advert : AggregateRoot
    {
        public AdvertId Id { get; init; } = null!;
        public UserId AuthorId { get; init; } = null!;
        public CategoryId CategoryId { get; private set; } = null!;

        public AdvertTitle Title { get; private set; } = null!;
        public AdvertDescription Description { get; private set; } = null!;
        public Price Price { get; private set; } = null!;

        public AdvertStatus Status { get; private set; }
        public DateTime CreatedAt { get; init; }
        public DateTime? PublishedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private Advert() { }

        public static Advert CreateDraft(
            UserId authorId, 
            CategoryId categoryId, 
            AdvertTitle title, 
            AdvertDescription description, 
            Price price) =>
            new()
            {
                Id = new AdvertId(Guid.NewGuid()),
                AuthorId = authorId,
                CategoryId = categoryId,
                Title = title,
                Description = description,
                Price = price,
                Status = AdvertStatus.Draft,
                CreatedAt = DateTime.UtcNow
            };
    }
}
