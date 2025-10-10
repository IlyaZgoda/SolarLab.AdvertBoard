using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.SharedKernel.Maybe;

namespace SolarLab.AdvertBoard.Domain.Comments
{
    public interface ICommentRepository
    {
        Task<Maybe<Comment>> GetByIdAsync(CommentId id);
        void Add(Comment comment);
        void Update(Comment comment);
        void Delete(Comment comment);
        Task DeleteAllForAdvert(AdvertId advertId);
    }
}
