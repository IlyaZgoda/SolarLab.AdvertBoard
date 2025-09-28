using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.SharedKernel.Maybe;

namespace SolarLab.AdvertBoard.Persistence.Repositories
{
    public class UserRepostory(ApplicationDbContext context) : IUserRepository
    {
        public async Task<Maybe<User>> GetByIdAsync(UserId id) =>
            await context.AppUsers.FirstOrDefaultAsync(u => u.Id == id);

        public void Add(User user) => context.Add(user);

        public void Update(User user) => context.Update(user);
        
    }
}
