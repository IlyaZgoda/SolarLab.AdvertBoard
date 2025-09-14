using MediatR;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Abstractions.Messaging
{
    public interface IQueryHandler<TQuery, TResponse>
        : IRequestHandler<TQuery, Result<TResponse>>
        where TQuery : IQuery<TResponse>
    {
    }
}
