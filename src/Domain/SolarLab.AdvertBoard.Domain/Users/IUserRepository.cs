using SolarLab.AdvertBoard.SharedKernel.Maybe;

namespace SolarLab.AdvertBoard.Domain.Users
{
    public interface IUserRepository
    {
        void Add(User user);
        void Update(User user);
        Task<Maybe<User>> GetByIdAsync(UserId id);
        Task<Maybe<User>> GetByUserIdentityIdAsync(string identityId);
        Task<Maybe<string>> GetIdentityIdByUserIdAsync(UserId id);
    }
}
