using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Images;

namespace SolarLab.AdvertBoard.Application.Images.GetById
{
    public record GetImageByIdQuery(Guid Id) : IQuery<ImageResponse>;
}
