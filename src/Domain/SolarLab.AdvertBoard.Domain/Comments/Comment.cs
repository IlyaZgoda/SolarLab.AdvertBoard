using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Domain.Comments
{
    /// <summary>
    /// Представляет комментарий к объявлению.
    /// </summary>
    public class Comment : AggregateRoot
    {
        /// <summary>
        /// Идентификатор комментария.
        /// </summary>
        public CommentId Id { get; init; } = null!;

        /// <summary>
        /// Идентификатор объявления, к которому относится комментарий.
        /// </summary>
        public AdvertId AdvertId { get; init; } = null!;

        /// <summary>
        /// Идентификатор автора комментария.
        /// </summary>
        public UserId AuthorId { get; init; } = null!;

        /// <summary>
        /// Текст комментария.
        /// </summary>
        public CommentText Text { get; private set; } = null!;

        /// <summary>
        /// Дата и время создания комментария.
        /// </summary>
        public DateTime CreatedAt { get; init; }

        /// <summary>
        /// Дата и время последнего обновления комментария.
        /// </summary>
        /// <value>Null если комментарий не редактировался.</value>
        public DateTime? UpdatedAt { get; private set; }

        /// <summary>
        /// Приватный конструктор для EF Core.
        /// </summary>
        private Comment() { }

        /// <summary>
        /// Создает новый комментарий.
        /// </summary>
        /// <param name="advertId">Идентификатор объявления.</param>
        /// <param name="authorId">Идентификатор автора.</param>
        /// <param name="text">Текст комментария.</param>
        /// <returns>Новый экземпляр комментария.</returns>
        public static Comment Create(
            AdvertId advertId,
            UserId authorId,
            CommentText text) => new()
            {
                Id = new CommentId(Guid.NewGuid()),
                AdvertId = advertId,
                AuthorId = authorId,
                Text = text,
                CreatedAt = DateTime.UtcNow,
            };

        /// <summary>
        /// Обновляет текст комментария.
        /// </summary>
        /// <param name="text">Новый текст комментария.</param>
        public void Update(CommentText text)
        {
            Text = text;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
