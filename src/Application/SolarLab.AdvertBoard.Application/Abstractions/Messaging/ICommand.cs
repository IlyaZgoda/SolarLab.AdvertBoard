using SolarLab.AdvertBoard.SharedKernel.Result;
using MediatR;

namespace SolarLab.AdvertBoard.Application.Abstractions.Messaging
{
    /// <summary>
    /// Маркерный интерфейс для команд без возвращаемого значения.
    /// </summary>
    /// <remarks>
    /// Наследуется от MediatR IRequest с типом Result для единообразной обработки ошибок.
    /// </remarks>
    public interface ICommand : IRequest<Result>
    {
    }

    /// <summary>
    /// Маркерный интерфейс для команд с возвращаемым значением.
    /// </summary>
    /// <typeparam name="TResponse">Тип возвращаемого значения.</typeparam>
    /// <remarks>
    /// Наследуется от MediatR IRequest с типом Result<TResponse> для единообразной обработки ошибок.
    /// </remarks>
    public interface ICommand<TResponse> : IRequest<Result<TResponse>>
    {
    }
}
