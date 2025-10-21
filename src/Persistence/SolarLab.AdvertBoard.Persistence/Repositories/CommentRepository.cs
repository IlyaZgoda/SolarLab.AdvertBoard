using Microsoft.EntityFrameworkCore;
using SolarLab.AdvertBoard.Domain.Comments;
using SolarLab.AdvertBoard.SharedKernel.Maybe;

namespace SolarLab.AdvertBoard.Persistence.Repositories
{
    /// <summary>
    /// Репозиторий для работы с комментариями.
    /// </summary>
    /// <param name="context"></param>
    public class CommentRepository(ApplicationDbContext context) : ICommentRepository
    {
        /// <inheritdoc/>
        public void Add(Comment comment) => context.Add(comment);

        /// <inheritdoc/>
        public void Delete(Comment comment) => context.Remove(comment);

        /// <inheritdoc/>
        public async Task<Maybe<Comment>> GetByIdAsync(CommentId id) =>
            await context.Comments.FirstOrDefaultAsync(u => u.Id == id);

        /// <inheritdoc/>
        public void Update(Comment comment) => context.Update(comment);
    }
}
