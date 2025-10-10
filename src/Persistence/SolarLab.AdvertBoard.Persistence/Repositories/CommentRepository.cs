using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Domain.Comments;
using SolarLab.AdvertBoard.SharedKernel.Maybe;

namespace SolarLab.AdvertBoard.Persistence.Repositories
{
    public class CommentRepository(ApplicationDbContext context) : ICommentRepository
    {
        public void Add(Comment comment) => context.Add(comment);

        public void Delete(Comment comment) => context.Remove(comment);

        public async Task<Maybe<Comment>> GetByIdAsync(CommentId id) =>
            await context.Comments.FirstOrDefaultAsync(u => u.Id == id);

        public void Update(Comment comment) => context.Update(comment);
    }
}
