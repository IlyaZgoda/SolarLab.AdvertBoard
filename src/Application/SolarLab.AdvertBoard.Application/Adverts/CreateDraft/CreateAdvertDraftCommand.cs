using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Adverts;

namespace SolarLab.AdvertBoard.Application.Adverts.CreateDraft
{
    /// <summary>
    /// Команда для создания черновика объявления.
    /// </summary>
    /// <param name="CategoryId">Идентификатор категории.</param>
    /// <param name="Title">Заголовок объявления.</param>
    /// <param name="Description">Описание объявления.</param>
    /// <param name="Price">Цена.</param>
    public record CreateAdvertDraftCommand(Guid CategoryId, string Title, string Description, decimal Price) : ICommand<AdvertIdResponse>;
}
