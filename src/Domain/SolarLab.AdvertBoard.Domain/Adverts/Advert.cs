using SolarLab.AdvertBoard.Domain.AdvertImages;
using SolarLab.AdvertBoard.Domain.Categories;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.Domain.Exceptions;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Domain.Adverts
{
    /// <summary>
    /// Представляет объявление в системе.
    /// </summary>
    public class Advert : AggregateRoot
    {
        /// <summary>
        /// Идентификатор объявления.
        /// </summary>
        public AdvertId Id { get; init; } = null!;

        /// <summary>
        /// Идентификатор автора объявления.
        /// </summary>
        public UserId AuthorId { get; init; } = null!;

        /// <summary>
        /// Идентификатор категории объявления.
        /// </summary>
        public CategoryId CategoryId { get; private set; } = null!;

        /// <summary>
        /// Заголовок объявления.
        /// </summary>
        public AdvertTitle Title { get; private set; } = null!;

        /// <summary>
        /// Описание объявления.
        /// </summary>
        public AdvertDescription Description { get; private set; } = null!;

        /// <summary>
        /// Цена в объявлении.
        /// </summary>
        public Price Price { get; private set; } = null!;

        /// <summary>
        /// Текущий статус объявления.
        /// </summary>
        public AdvertStatus Status { get; private set; }

        /// <summary>
        /// Дата и время создания объявления.
        /// </summary>
        public DateTime CreatedAt { get; init; }

        /// <summary>
        /// Дата и время публикации объявления.
        /// </summary>
        public DateTime? PublishedAt { get; private set; }

        /// <summary>
        /// Дата и время последнего обновления объявления.
        /// </summary>
        public DateTime? UpdatedAt { get; private set; }

        private readonly List<AdvertImage> _images = [];

        /// <summary>
        /// Коллекция изображений объявления.
        /// </summary>
        public IReadOnlyCollection<AdvertImage> Images => _images.AsReadOnly();


        /// <summary>
        /// Максимальное количество изображений для одного объявления.
        /// </summary>
        public const int MaxImagesCount = 5;

        /// <summary>
        /// Приватный конструктор для EF Core.
        /// </summary>
        private Advert() { }

        /// <summary>
        /// Добавляет изображение к объявлению.
        /// </summary>
        /// <param name="fileName">Имя файла изображения.</param>
        /// <param name="contentType">MIME-тип содержимого изображения.</param>
        /// <param name="imageContent">Бинарное содержимое изображения.</param>
        /// <returns>
        /// Успешный результат с идентификатором добавленного изображения или ошибку:
        /// - <see cref="AdvertImageErrors.CantAddImageToNonDraftAdvert"/> если объявление не в черновике
        /// - <see cref="AdvertImageErrors.TooManyImages"/> если достигнут лимит изображений
        /// </returns>
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

        /// <summary>
        /// Удаляет изображение из объявления.
        /// </summary>
        /// <param name="advertImageId">Идентификатор изображения для удаления.</param>
        /// <returns>
        /// Успешный результат или ошибку:
        /// - <see cref="AdvertImageErrors.CantDeleteImageFromNonDraftAdvert"/> если объявление не в черновике
        /// - <see cref="AdvertImageErrors.NotFound"/> если изображение не найдено
        /// </returns>
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

        /// <summary>
        /// Публикует объявление.
        /// </summary>
        /// <exception cref="DomainException">
        /// Вызывается когда:
        /// - объявление не в статусе черновика (<see cref="AdvertErrors.CantPublishNonDraftAdvert"/>)
        /// - у объявления нет изображений (<see cref="AdvertErrors.CantPublishWithNoImage"/>)
        /// </exception>
        public void Publish()
        {
            if (Status != AdvertStatus.Draft)
            {
                throw new DomainException(AdvertErrors.CantPublishNonDraftAdvert.Description);
            }

            if (_images.Count == 0)
            {
                throw new DomainException(AdvertErrors.CantPublishWithNoImage.Description);
            }

            Status = AdvertStatus.Published;
            PublishedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Снимает объявление с публикации.
        /// </summary>
        /// <exception cref="DomainException">
        /// Вызывается когда объявление не в статусе опубликованного (<see cref="AdvertErrors.CanOnlyUnpublishPublishedAdverts"/>)
        /// </exception>
        public void Unpublish()
        {
            if (Status != AdvertStatus.Published)
            {
                throw new DomainException(AdvertErrors.CanOnlyUnpublishPublishedAdverts.Description);
            }
        }

        /// <summary>
        /// Удаляет черновик объявления.
        /// </summary>
        /// <exception cref="DomainException">
        /// Вызывается когда объявление не в статусе черновика (<see cref="AdvertErrors.CanOnlyUnpublishPublishedAdverts"/>)
        /// </exception>
        public void DeleteDraft()
        {
            if (Status != AdvertStatus.Draft)
            {
                throw new DomainException(AdvertErrors.CanOnlyUnpublishPublishedAdverts.Description);
            }
        }

        /// <summary>
        /// Создает новый черновик объявления.
        /// </summary>
        /// <param name="authorId">Идентификатор автора.</param>
        /// <param name="categoryId">Идентификатор категории.</param>
        /// <param name="title">Заголовок объявления.</param>
        /// <param name="description">Описание объявления.</param>
        /// <param name="price">Цена.</param>
        /// <returns>Новый экземпляр черновика объявления.</returns>
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

        /// <summary>
        /// Обновляет черновик объявления.
        /// </summary>
        /// <param name="categoryId">Новая категория (опционально).</param>
        /// <param name="title">Новый заголовок (опционально).</param>
        /// <param name="description">Новое описание (опционально).</param>
        /// <param name="price">Новая цена (опционально).</param>
        /// <exception cref="DomainException">
        /// Вызывается когда:
        /// - объявление не в статусе черновика (<see cref="AdvertErrors.CantUpdateNonDraftAdvert"/>)
        /// - не передано ни одного изменяемого параметра (<see cref="AdvertErrors.NoChanges"/>)
        /// - ни одно из переданных значений не отличается от текущих (<see cref="AdvertErrors.NoChanges"/>)
        /// </exception>
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
