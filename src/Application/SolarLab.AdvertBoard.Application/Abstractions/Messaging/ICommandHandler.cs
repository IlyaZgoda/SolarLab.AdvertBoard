using MediatR;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Abstractions.Messaging
{
    /// <summary>
    /// Обработчик команд без возвращаемого значения.
    /// </summary>
    /// <typeparam name="TCommand">Тип команды.</typeparam>
    /// <remarks>
    /// Наследуется от MediatR IRequestHandler с типом Result для единообразной обработки ошибок.
    /// </remarks>
    public interface ICommandHandler<TCommand>
       : IRequestHandler<TCommand, Result>
       where TCommand : ICommand
    {
    }

    /// <summary>
    /// Обработчик команд с возвращаемым значением.
    /// </summary>
    /// <typeparam name="TCommand">Тип команды.</typeparam>
    /// <typeparam name="TResponse">Тип возвращаемого значения.</typeparam>
    /// <remarks>
    /// Наследуется от MediatR IRequestHandler с типом Result<TResponse> для единообразной обработки ошибок.
    /// </remarks>
    public interface ICommandHandler<TCommand, TResponse>
        : IRequestHandler<TCommand, Result<TResponse>>
        where TCommand : ICommand<TResponse>
    {
    }
}
