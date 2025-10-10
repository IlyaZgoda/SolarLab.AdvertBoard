using SolarLab.AdvertBoard.Application.Abstractions.Messaging;
using SolarLab.AdvertBoard.Contracts.Comments;
using SolarLab.AdvertBoard.Domain.Comments;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Application.Comments.Get
{
    public class GetCommentByIdQueryHandler(ICommentRepository commentRepository) 
        : IQueryHandler<GetCommentByIdQuery, CommentResponse>
    {
        public async Task<Result<CommentResponse>> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
        {
            var comment = await commentRepository.GetByIdAsync(new CommentId(request.Id));

            if (comment.HasNoValue)
            {
                return Result.Failure<CommentResponse>(CommentErrors.NotFound);
            }

            return new CommentResponse(
                comment.Value.Id, 
                comment.Value.AdvertId, 
                comment.Value.AuthorId, 
                comment.Value.Text.Value, 
                comment.Value.CreatedAt, 
                comment.Value.UpdatedAt);
        }
    }
}
