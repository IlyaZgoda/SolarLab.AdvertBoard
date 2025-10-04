using SolarLab.AdvertBoard.Domain.Categories;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.Domain.Exceptions;
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

        public void Publish()
        {
            if (Status != AdvertStatus.Draft)
            {
                throw new DomainException(AdvertErrors.CantPublishNonDraftAdvert);
            }

            Status = AdvertStatus.Published;
            PublishedAt = DateTime.UtcNow;
        }

        public void Archive()
        {
            if (Status != AdvertStatus.Published)
            {
                throw new DomainException(AdvertErrors.CantArchiveNonPublishedAdvert);
            }

            Status = AdvertStatus.Archived;
        }

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

        public void UpdateDraft(
            CategoryId? categoryId, 
            AdvertTitle? title, 
            AdvertDescription? description, 
            Price? price)
        {
            if (Status != AdvertStatus.Draft)
            {
                throw new DomainException(AdvertErrors.CantUpdateNonDraftAdvert);
            }

            if (AllNull(categoryId, title, description, price) 
                || !AnyChanged(
                (CategoryId, categoryId), 
                (Title, title), 
                (Description, description), 
                (Price, price)))
            {
                throw new DomainException(AdvertErrors.NoChanges.Description);
            }

            if (categoryId != null)
            {
                CategoryId = categoryId;
            }

            if (title != null)
            {
                Title = title;
            }

            if (description != null)
            {
                Description = description;
            }

            if (price != null)
            {
                Price = price;
            }

            UpdatedAt = DateTime.UtcNow;
        }

        private static bool AllNull(params IValueObject?[] objects) =>
            objects.All(o => o is null);

        private static bool AnyChanged(params (IValueObject, IValueObject?)[] pairs) =>
            pairs.Any(p => !p.Item1.Equals(p.Item2));
    }
}
