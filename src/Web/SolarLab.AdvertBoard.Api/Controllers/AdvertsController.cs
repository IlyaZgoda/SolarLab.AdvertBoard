using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarLab.AdvertBoard.Api.Mappers;
using SolarLab.AdvertBoard.Application.Adverts.CreateDraft;
using SolarLab.AdvertBoard.Contracts.Adverts;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;
using System.Security.Claims;

namespace SolarLab.AdvertBoard.Api.Controllers
{
    public class AdvertsController(
        IMediator mediator,
        ResultErrorHandler resultErrorHandler) : ControllerBase
    {
        [Authorize]
        [HttpPost(ApiRoutes.Adverts.CreateDraft)]
        [ProducesResponseType(typeof(AdvertIdResponse), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> CreateDraft(CreateDraftRequest createDraftRequest) =>
            await Result.Create(createDraftRequest, Error.None)
                .Map(request => new CreateAdvertDraftCommand(
                    request.CategoryId, request.Title, request.Description, request.Price))
                .Bind(command => mediator.Send(command))
                .Match(token => Ok(token), error => resultErrorHandler.Handle(error));

        [Authorize]
        [HttpPost("test-auth")]
        public IActionResult TestAuth()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            return Ok(new
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                UserId = userId,
                Email = userEmail,
                Claims = User.Claims.Select(c => new { c.Type, c.Value })
            });
        }
    }
}

