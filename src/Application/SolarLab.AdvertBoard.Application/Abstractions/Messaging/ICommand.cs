using SolarLab.AdvertBoard.SharedKernel.Result;
using MediatR;

namespace SolarLab.AdvertBoard.Application.Abstractions.Messaging
{
    public interface ICommand : IRequest<Result>
    {
    }

    public interface ICommand<TResponse> : IRequest<Result<TResponse>>
    {
    }
}
