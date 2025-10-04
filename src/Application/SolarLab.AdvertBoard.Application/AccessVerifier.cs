using SolarLab.AdvertBoard.Domain.Users;

namespace SolarLab.AdvertBoard.Application
{
    public class AccessVerifier(IUserRepository userRepository)
    {
        public async Task<bool> HasAccess(UserId userId, string identityId) =>
            await userRepository.GetIdentityIdByUserIdAsync(userId) == identityId;
    }
}
