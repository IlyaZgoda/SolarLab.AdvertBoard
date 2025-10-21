using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Application.Abstractions.Read.Providers;
using SolarLab.AdvertBoard.Contracts.Base;
using SolarLab.AdvertBoard.Contracts.Comments;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Comments.GetByAdvertId
{
    /// <summary>
    /// Обработчик запроса <see cref="GetCommentsByAdvertIdQuery"/>.
    /// </summary>
    /// <param name="commentReadProvider">Првоайдер для чтения данных комментариев.</param>
    public class GetCommentsByAdvertIdQueryHandler(ICommentReadProvider commentReadProvider) 
        : IQueryHandler<GetCommentsByAdvertIdQuery, PaginationCollection<CommentItem>>
    {
        /// <inheritdoc/>
        public async Task<Result<PaginationCollection<CommentItem>>> Handle(
            GetCommentsByAdvertIdQuery request, CancellationToken cancellationToken) => 
            await commentReadProvider.GetCommentsByIdAsync(request.AdvertId, request.Page, request.PageSize);
    }
}
