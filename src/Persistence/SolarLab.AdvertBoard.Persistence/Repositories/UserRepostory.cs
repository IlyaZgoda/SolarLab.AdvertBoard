using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Application.Users.Specifications;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel.Maybe;
using SolarLab.AdvertBoard.SharedKernel.Specification;

namespace SolarLab.AdvertBoard.Persistence.Repositories
{
    /// <summary>
    /// Репозиторий для работы с пользователями.
    /// </summary>
    /// <param name="context">Контектс (для записи) базы данных.</param>
    public class UserRepostory(ApplicationDbContext context) : IUserRepository
    {
        /// <inheritdoc/>
        public async Task<Maybe<User>> GetByIdAsync(UserId id) =>
            await context.AppUsers.FirstOrDefaultAsync(u => u.Id == id);

        /// <inheritdoc/>
        public void Add(User user) => context.Add(user);

        /// <inheritdoc/>
        public void Update(User user) => context.Update(user);

        /// <inheritdoc/>
        public async Task<Maybe<User>> GetBySpecificationAsync(Specification<User> specification) => 
            await context.AppUsers.Where(specification).FirstOrDefaultAsync();

        /// <inheritdoc/>
        public async Task<bool> IsOwner(UserId userId, string identityId) =>
            await context.AppUsers.AnyAsync(
                new UserWithIdentitySpecification(identityId)
                .And(new UserWithIdSpecification(userId)));
    }
}
