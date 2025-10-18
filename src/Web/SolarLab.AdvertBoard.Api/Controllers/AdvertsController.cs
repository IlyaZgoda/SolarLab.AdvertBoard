using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarLab.AdvertBoard.Api.Mappers;
using SolarLab.AdvertBoard.Application.Adverts.CreateDraft;
using SolarLab.AdvertBoard.Application.Adverts.DeleteDraft;
using SolarLab.AdvertBoard.Application.Adverts.DeletePublished;
using SolarLab.AdvertBoard.Application.Adverts.GetDraftById;
using SolarLab.AdvertBoard.Application.Adverts.GetPublishedAdvertDetailsById;
using SolarLab.AdvertBoard.Application.Adverts.GetPublishedAdvertsByFilter;
using SolarLab.AdvertBoard.Application.Adverts.GetUserDrafts;
using SolarLab.AdvertBoard.Application.Adverts.GetUserPublishedAdverts;
using SolarLab.AdvertBoard.Application.Adverts.PublishDraft;
using SolarLab.AdvertBoard.Application.Adverts.UpdateDraft;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.Contracts.Base;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;

namespace SolarLab.AdvertBoard.Api.Controllers
{
    [Authorize]
    public class AdvertsController(
        IMediator mediator,
        ResultErrorHandler resultErrorHandler) : ControllerBase
    {
        [HttpPost(ApiRoutes.Adverts.CreateDraft)]
        [ProducesResponseType(typeof(AdvertIdResponse), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 422)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> CreateDraft(CreateAdvertDraftRequest createDraftRequest) =>
            await Result.Create(createDraftRequest, Error.None)
                .Map(request => new CreateAdvertDraftCommand(
                    request.CategoryId, request.Title, request.Description, request.Price))
                .Bind(command => mediator.Send(command))
                .Match(id => Ok(id), error => resultErrorHandler.Handle(error));

        [HttpPatch(ApiRoutes.Adverts.UpdateDraft)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 422)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> UpdateDraft(Guid id, UpdateAdvertDraftRequest updateDraftRequest) =>
            await Result.Create(updateDraftRequest, Error.None)
                .Map(request => new UpdateAdvertDraftCommand(
                    id, request.CategoryId, request.Title, request.Description, request.Price))
                .Bind(command => mediator.Send(command))
                .Match(NoContent, error => resultErrorHandler.Handle(error));

        [HttpGet(ApiRoutes.Adverts.GetDraft)]
        [ProducesResponseType(typeof(AdvertDraftDetailsResponse), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> GetDraftAdvertById(Guid id) =>
            await Result.Create(new GetAdvertDraftByIdQuery(id), Error.None)
                .Map(request => new GetAdvertDraftByIdQuery(request.Id))
                .Bind(command => mediator.Send(command))
                .Match(response => Ok(response), error => resultErrorHandler.Handle(error));

        [HttpDelete(ApiRoutes.Adverts.DeleteDraft)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> DeleteById(Guid id) =>
            await Result.Create(new DeleteAdvertDraftCommand(id), Error.None)
                .Map(request => new DeleteAdvertDraftCommand(request.Id))
                .Bind(command => mediator.Send(command))
                .Match(NoContent, error => resultErrorHandler.Handle(error));

        [HttpPatch(ApiRoutes.Adverts.PublishDraft)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> PublishDraft(Guid id) =>
            await Result.Create(new PublishAdvertDraftCommand(id), Error.None)
                .Map(request => new PublishAdvertDraftCommand(request.Id))
                .Bind(command => mediator.Send(command))
                .Match(NoContent, error => resultErrorHandler.Handle(error));

        [HttpDelete(ApiRoutes.Adverts.DeletePublished)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> DeletePublished(Guid id) =>
            await Result.Create(new DeletePublishedAdvertCommand(id), Error.None)
                .Map(request => new DeletePublishedAdvertCommand(request.Id))
                .Bind(command => mediator.Send(command))
                .Match(NoContent, error => resultErrorHandler.Handle(error));

        [AllowAnonymous] //ДЛЯ ТЕСТА
        [HttpGet(ApiRoutes.Adverts.GetPublished)]
        [ProducesResponseType(typeof(PublishedAdvertDetailsResponse), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> GetPublishedAdvertById(Guid id) =>
            await Result.Create(new GetPublishedAdvertDetailsByIdQuery(id), Error.None)
                .Map(request => new GetPublishedAdvertDetailsByIdQuery(request.Id))
                .Bind(command => mediator.Send(command))
                .Match(response => Ok(response), error => resultErrorHandler.Handle(error));

        [AllowAnonymous] //ДЛЯ ТЕСТА
        [HttpGet(ApiRoutes.Adverts.GetPublishedByFilter)]
        [ProducesResponseType(typeof(PaginationCollection<PublishedAdvertItem>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> GetPublishedAdvertsByFilter(AdvertFilterRequest filter) =>
            await Result.Create(filter, Error.None)
                .Map(request => new GetPublishedAdvertsByFilterQuery(filter))
                .Bind(command => mediator.Send(command))
                .Match(response => Ok(response), error => resultErrorHandler.Handle(error));

        [HttpGet(ApiRoutes.Adverts.GetMyDrafts)]
        [ProducesResponseType(typeof(PaginationCollection<AdvertDraftItem>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> GetMyAdvertDrafts(GetUserAdvertDraftsRequest getUserAdvertDraftsRequest) =>
            await Result.Create(getUserAdvertDraftsRequest, Error.None)
                .Map(request => new GetUserAdvertDraftsQuery(request.Page, request.PageSize))
                .Bind(command => mediator.Send(command))
                .Match(response => Ok(response), error => resultErrorHandler.Handle(error));

        [HttpGet(ApiRoutes.Adverts.GetMyPublished)]
        [ProducesResponseType(typeof(PaginationCollection<PublishedAdvertItem>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> GetMyPublishedAdverts(GetUserPublishedAdvertsRequest getUserPublishedAdvertsRequest) =>
            await Result.Create(getUserPublishedAdvertsRequest, Error.None)
                .Map(request => new GetUserPublishedAdvertsQuery(request.Page, request.PageSize))
                .Bind(command => mediator.Send(command))
                .Match(response => Ok(response), error => resultErrorHandler.Handle(error));
    }
}

