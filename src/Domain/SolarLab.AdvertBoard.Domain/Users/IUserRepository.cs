using SolarLab.AdvertBoard.SharedKernel.Maybe;
using SolarLab.AdvertBoard.SharedKernel.Specification;

namespace SolarLab.AdvertBoard.Domain.Users
{
    public interface IUserRepository
    {
        void Add(User user);
        void Update(User user);
        Task<Maybe<User>> GetByIdAsync(UserId id);
        Task<Maybe<User>> GetBySpecificationAsync(Specification<User> specification);
        Task<bool> IsOwner(UserId userId, string identityId);
    }
}
