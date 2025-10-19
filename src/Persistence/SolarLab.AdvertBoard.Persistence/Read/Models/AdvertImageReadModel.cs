using SolarLab.AdvertBoard.Application.Abstractions.Read.Models;

namespace SolarLab.AdvertBoard.Persistence.Read.Models
{
    public class AdvertImageReadModel : IAdvertImageReadModel
    {
        public Guid Id { get; }
        public Guid AdvertId { get; }
        public AdvertReadModel Advert { get; } = null!;

        IAdvertReadModel IAdvertImageReadModel.Advert => Advert; 

        public string FileName { get; } = null!;
        public string ContentType { get; } = null!;
        public byte[] Content { get; } = null!;
        public DateTime CreatedAt { get; }
    }

}
