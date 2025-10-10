using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Application.Abstractions.ReadProviders;
using SolarLab.AdvertBoard.Contracts.Base;
using SolarLab.AdvertBoard.Contracts.Comments;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Comments.GetByAdvertId
{
    public class GetCommentByAdvertIdQueryHandler(ICommentReadProvider commentReadProvider) 
        : IQueryHandler<GetCommentsByAdvertIdQuery, PaginationCollection<CommentItem>>
    {
        public async Task<Result<PaginationCollection<CommentItem>>> Handle(
            GetCommentsByAdvertIdQuery request, CancellationToken cancellationToken) => 
            await commentReadProvider.GetCommentsByIdAsync(request.AdvertId, request.Page, request.PageSize);
    }
}
