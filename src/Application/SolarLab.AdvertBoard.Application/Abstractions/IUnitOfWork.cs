namespace SolarLab.AdvertBoard.Application.Abstractions
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
