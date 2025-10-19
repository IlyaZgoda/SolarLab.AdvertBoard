using SolarLab.AdvertBoard.Domain.AdvertImages;
using SolarLab.AdvertBoard.Domain.Categories;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.Domain.Exceptions;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;

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

        private readonly List<AdvertImage> _images = [];
        public IReadOnlyCollection<AdvertImage> Images => _images.AsReadOnly();
        public const int MaxImagesCount = 5;

        private Advert() { }

        public Result<AdvertImageId> AddImage(ImageFileName fileName, ImageContentType contentType, ImageContent imageContent)
        {
            if (Status != AdvertStatus.Draft)
            {
                return Result.Failure<AdvertImageId>(AdvertImageErrors.CantAddImageToNonDraftAdvert);
            }

            if (_images.Count >= MaxImagesCount)
            {
                return Result.Failure<AdvertImageId>(AdvertImageErrors.TooManyImages);
            }

            var image = AdvertImage.Create(Id, fileName, contentType, imageContent);

            _images.Add(image);

            UpdatedAt = DateTime.UtcNow;

            return Result.Success(image.Id);
        }

        public Result DeleteImage(AdvertImageId advertImageId)
        {

            if (Status != AdvertStatus.Draft)
            {
                return Result.Failure<AdvertImageId>(AdvertImageErrors.CantDeleteImageFromNonDraftAdvert);
            }

            var image = _images.Find(i => i.Id == advertImageId);

            if (image is null)
            {
                return Result.Failure<AdvertImageId>(AdvertImageErrors.NotFound);
            }

            _images.Remove(image);

            UpdatedAt = DateTime.UtcNow;

            return Result.Success();
        }

        public void Publish()
        {
            if (Status != AdvertStatus.Draft)
            {
                throw new DomainException(AdvertErrors.CantPublishNonDraftAdvert.Description);
            }

            Status = AdvertStatus.Published;
            PublishedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Unpublish()
        {
            if (Status != AdvertStatus.Published)
            {
                throw new DomainException(AdvertErrors.CanOnlyUnpublishPublishedAdverts.Description);
            }
        }

        public void DeleteDraft()
        {
            if (Status != AdvertStatus.Draft)
            {
                throw new DomainException(AdvertErrors.CanOnlyUnpublishPublishedAdverts.Description);
            }
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
                throw new DomainException(AdvertErrors.CantUpdateNonDraftAdvert.Description);
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
