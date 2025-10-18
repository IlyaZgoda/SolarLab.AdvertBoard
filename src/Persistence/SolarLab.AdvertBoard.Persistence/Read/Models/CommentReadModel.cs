namespace SolarLab.AdvertBoard.Persistence.Read.Models
{
    public class CommentReadModel
    {
        public Guid Id { get; set; }
        public Guid AdvertId { get; set; }
        public AdvertReadModel Advert { get; set; } = null!;

        public Guid AuthorId { get; set; }
        public UserReadModel Author { get; set; } = null!;

        public string Text { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

}
