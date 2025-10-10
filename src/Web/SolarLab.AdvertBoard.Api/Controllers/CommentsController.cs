using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarLab.AdvertBoard.Api.Mappers;
using SolarLab.AdvertBoard.Application.Comments.Create;
using SolarLab.AdvertBoard.Application.Comments.Delete;
using SolarLab.AdvertBoard.Application.Comments.GetByAdvertId;
using SolarLab.AdvertBoard.Application.Comments.GetById;
using SolarLab.AdvertBoard.Application.Comments.Update;
using SolarLab.AdvertBoard.Contracts.Base;
using SolarLab.AdvertBoard.Contracts.Comments;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;

namespace SolarLab.AdvertBoard.Api.Controllers
{
    [Authorize]
    public class CommentsController(
        IMediator mediator,
        ResultErrorHandler resultErrorHandler) : ControllerBase
    {
        [HttpPost(ApiRoutes.Comments.Create)]
        [ProducesResponseType(typeof(CommentIdResponse), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 422)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> Create(Guid advertId, CreateCommentRequest createCommentRequest) =>
            await Result.Create(createCommentRequest, Error.None)
                .Map(request => new CreateCommentCommand(advertId, request.Text))
                .Bind(command => mediator.Send(command))
                .Match(id => Ok(id), error => resultErrorHandler.Handle(error));

        [HttpPut(ApiRoutes.Comments.Update)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 422)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> Update(Guid commentId, UpdateCommentRequest updateCommentRequest) =>
            await Result.Create(updateCommentRequest, Error.None)
                .Map(request => new UpdateCommentCommand(commentId, request.Text))
                .Bind(command => mediator.Send(command))
                .Match(NoContent, error => resultErrorHandler.Handle(error));

        [HttpDelete(ApiRoutes.Comments.Delete)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> Delete(Guid commentId) =>
            await Result.Create(new DeleteCommentCommand(commentId), Error.None)
                .Map(request => new DeleteCommentCommand(commentId))
                .Bind(command => mediator.Send(command))
                .Match(NoContent, error => resultErrorHandler.Handle(error));

        [HttpGet(ApiRoutes.Comments.GetById)]
        [ProducesResponseType(typeof(CommentResponse), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> GetCommentById(Guid commentId) =>
            await Result.Create(new GetCommentByIdQuery(commentId), Error.None)
                .Map(request => new GetCommentByIdQuery(request.Id))
                .Bind(command => mediator.Send(command))
                .Match(response => Ok(response), error => resultErrorHandler.Handle(error));


        [HttpGet(ApiRoutes.Comments.GetByAdvertId)]
        [ProducesResponseType(typeof(PaginationCollection<CommentItem>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> GetCommentsByAdvertId(Guid advertId, GetCommentsByAdvertIdRequest getCommentsByAdvertIdRequest) =>
            await Result.Create(getCommentsByAdvertIdRequest, Error.None)
                .Map(request => new GetCommentsByAdvertIdQuery(advertId, request.Page, request.PageSize))
                .Bind(command => mediator.Send(command))
                .Match(response => Ok(response), error => resultErrorHandler.Handle(error));
    }
}

