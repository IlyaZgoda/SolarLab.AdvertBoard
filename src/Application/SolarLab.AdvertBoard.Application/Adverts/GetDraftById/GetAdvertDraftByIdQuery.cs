using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Adverts;

namespace SolarLab.AdvertBoard.Application.Adverts.GetDraftById
{
    /// <summary>
    /// Запрос для получения черновика объявления по идентификатору.
    /// </summary>
    /// <param name="Id">Идентификатор черновика.</param>
    public record GetAdvertDraftByIdQuery(Guid Id) : IQuery<AdvertDraftDetailsResponse>;
}
