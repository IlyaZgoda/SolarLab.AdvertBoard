using MediatR;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Abstractions.Messaging
{
    /// <summary>
    /// Маркерный интерфейс для запросов с возвращаемым значением.
    /// </summary>
    /// <typeparam name="TResponse">Тип возвращаемого значения.</typeparam>
    /// <remarks>
    /// Наследуется от MediatR IRequest с типом Result<TResponse> для единообразной обработки ошибок.
    /// </remarks>
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {
    }
}
