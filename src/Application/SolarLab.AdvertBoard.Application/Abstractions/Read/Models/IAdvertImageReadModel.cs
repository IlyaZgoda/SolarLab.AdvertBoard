namespace SolarLab.AdvertBoard.Application.Abstractions.Read.Models
{
    public interface IAdvertImageReadModel
    {
        public Guid Id { get; }
        public Guid AdvertId { get; }
        public IAdvertReadModel Advert { get; }

        public string FileName { get; }
        public string ContentType { get; }
        public byte[] Content { get; }
        public DateTime CreatedAt { get; }
    }
}
