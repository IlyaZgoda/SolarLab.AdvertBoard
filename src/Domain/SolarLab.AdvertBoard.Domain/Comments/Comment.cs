using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Domain.Comments
{
    public class Comment : AggregateRoot
    {
        public CommentId Id { get; init; } = null!;
        public AdvertId AdvertId { get; init; } = null!;
        public UserId AuthorId { get; init; } = null!;
        public CommentText Text { get; private set; } = null!;
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; private set; }

        private Comment() { }

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

        public void Update(CommentText text)
        {
            Text = text;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
