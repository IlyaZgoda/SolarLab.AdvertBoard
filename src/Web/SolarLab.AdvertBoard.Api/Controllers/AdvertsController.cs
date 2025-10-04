using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SolarLab.AdvertBoard.Api.Mappers;
using SolarLab.AdvertBoard.Application.Adverts.Archive;
using SolarLab.AdvertBoard.Application.Adverts.CreateDraft;
using SolarLab.AdvertBoard.Application.Adverts.Delete;
using SolarLab.AdvertBoard.Application.Adverts.Get;
using SolarLab.AdvertBoard.Application.Adverts.PublishDraft;
using SolarLab.AdvertBoard.Application.Adverts.Update;
using SolarLab.AdvertBoard.Application.Categories.GetById;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;
using System.Security.Claims;

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
        [ProducesResponseType(typeof(UpdateAdvertDraftRequest), 200)]
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

        [HttpGet(ApiRoutes.Adverts.GetAdvertDraft)]
        [ProducesResponseType(typeof(UpdateAdvertDraftRequest), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> GetById(Guid id) =>
            await Result.Create(new GetAdvertDraftByIdQuery(id), Error.None)
                .Map(request => new GetAdvertDraftByIdQuery(request.Id))
                .Bind(command => mediator.Send(command))
                .Match(response => Ok(response), error => resultErrorHandler.Handle(error));

        [HttpPost(ApiRoutes.Adverts.DeleteAdvertDraft)]
        [ProducesResponseType(typeof(UpdateAdvertDraftRequest), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> DeleteById(Guid id) =>
            await Result.Create(new DeleteAdvertDraftCommand(id), Error.None)
                .Map(request => new DeleteAdvertDraftCommand(request.Id))
                .Bind(command => mediator.Send(command))
                .Match(NoContent, error => resultErrorHandler.Handle(error));

        [HttpPatch(ApiRoutes.Adverts.PublishDraft)]
        [ProducesResponseType(typeof(UpdateAdvertDraftRequest), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> PublishDraft(Guid id) =>
            await Result.Create(new PublishAdvertDraftCommand(id), Error.None)
                .Map(request => new PublishAdvertDraftCommand(request.Id))
                .Bind(command => mediator.Send(command))
                .Match(NoContent, error => resultErrorHandler.Handle(error));

        [HttpPatch(ApiRoutes.Adverts.Archive)]
        [ProducesResponseType(typeof(UpdateAdvertDraftRequest), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> Archive(Guid id) =>
            await Result.Create(new ArchiveAdvertCommand(id), Error.None)
                .Map(request => new ArchiveAdvertCommand(request.Id))
                .Bind(command => mediator.Send(command))
                .Match(NoContent, error => resultErrorHandler.Handle(error));
    }
}

