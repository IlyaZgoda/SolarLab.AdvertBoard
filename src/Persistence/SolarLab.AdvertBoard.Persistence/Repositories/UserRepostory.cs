using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Application.Users.Specifications;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel.Maybe;
using SolarLab.AdvertBoard.SharedKernel.Specification;

namespace SolarLab.AdvertBoard.Persistence.Repositories
{
    public class UserRepostory(ApplicationDbContext context) : IUserRepository
    {
        public async Task<Maybe<User>> GetByIdAsync(UserId id) =>
            await context.AppUsers.FirstOrDefaultAsync(u => u.Id == id);

        public void Add(User user) => context.Add(user);

        public void Update(User user) => context.Update(user);

        public async Task<Maybe<User>> GetBySpecificationAsync(Specification<User> specification) => 
            await context.AppUsers.Where(specification).FirstOrDefaultAsync();

        public async Task<bool> IsOwner(UserId userId, string identityId) =>
            await context.AppUsers.AnyAsync(
                new UserWithIdentitySpecification(identityId)
                .And(new UserWithIdSpecification(userId)));
    }
}
