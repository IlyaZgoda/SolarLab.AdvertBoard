namespace SolarLab.AdvertBoard.Persistence.Read.Models
{
    public class AdvertImageReadModel
    {
        public Guid Id { get; set; }
        public Guid AdvertId { get; set; }
        public AdvertReadModel Advert { get; set; } = null!;

        public string FileName { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public byte[] Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }

}
