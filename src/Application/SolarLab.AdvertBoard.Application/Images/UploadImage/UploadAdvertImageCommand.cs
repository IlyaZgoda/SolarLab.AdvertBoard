using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Images;

namespace SolarLab.AdvertBoard.Application.Images.UploadImage
{
    public record UploadAdvertImageCommand(Guid AdvertId, string FileName, string ContentType, byte[] Content) : ICommand<ImageIdResponse>;
}
