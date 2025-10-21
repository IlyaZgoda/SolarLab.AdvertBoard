using MediatR;

namespace SolarLab.AdvertBoard.SharedKernel
{
    /// <summary>
    /// Интерфейс для доменных событий.
    /// </summary>
    public interface IDomainEvent : INotification
    {
    }
}
