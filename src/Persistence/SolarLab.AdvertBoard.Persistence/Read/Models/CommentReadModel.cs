using SolarLab.AdvertBoard.Application.Abstractions.Read.Models;

namespace SolarLab.AdvertBoard.Persistence.Read.Models
{
    public class CommentReadModel : ICommentReadModel
    {
        public Guid Id { get; }
        public Guid AdvertId { get; }
        public AdvertReadModel Advert { get; } = null!;

        public Guid AuthorId { get; }
        public UserReadModel Author { get; } = null!;

        public string Text { get; } = null!;
        public DateTime CreatedAt { get; }
        public DateTime? UpdatedAt { get; }

        IAdvertReadModel ICommentReadModel.Advert => Advert;

        IUserReadModel ICommentReadModel.Author => Author;
    }

}
