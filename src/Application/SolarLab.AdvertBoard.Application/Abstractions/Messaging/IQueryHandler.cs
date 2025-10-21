using MediatR;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Abstractions.Messaging
{
    /// <summary>
    /// Обработчик запросов с возвращаемым значением.
    /// </summary>
    /// <typeparam name="TQuery">Тип запроса.</typeparam>
    /// <typeparam name="TResponse">Тип возвращаемого значения.</typeparam>
    /// <remarks>
    /// Наследуется от MediatR IRequestHandler с типом Result<TResponse> для единообразной обработки ошибок.
    /// </remarks>
    public interface IQueryHandler<TQuery, TResponse>
        : IRequestHandler<TQuery, Result<TResponse>>
        where TQuery : IQuery<TResponse>
    {
    }
}
