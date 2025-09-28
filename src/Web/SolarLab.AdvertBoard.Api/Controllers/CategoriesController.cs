using MediatR;
using Microsoft.AspNetCore.Mvc;
using SolarLab.AdvertBoard.Api.Mappers;
using SolarLab.AdvertBoard.Application.Categories.GetById;
using SolarLab.AdvertBoard.Application.Categories.GetTree;
using SolarLab.AdvertBoard.Contracts.Categories;
using SolarLab.AdvertBoard.Domain.Categories;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;

namespace SolarLab.AdvertBoard.Api.Controllers
{
    public class CategoriesController(
        IMediator mediator, 
        ResultErrorHandler resultErrorHandler) : ControllerBase
    {
        [HttpGet(ApiRoutes.Categories.GetById)]
        [ProducesResponseType(typeof(CategoryResponse), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> GetById(Guid id) =>
            await Result.Create(new GetCategoryByIdQuery(id), Error.None)
                .Map(request => new GetCategoryByIdQuery(id))
                .Bind(command => mediator.Send(command))
                .Match(token => Ok(token), error => resultErrorHandler.Handle(error));

        [HttpGet(ApiRoutes.Categories.GetTree)]
        [ProducesResponseType(typeof(CategoryTreeResponse), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> GetTree() =>
            await Result.Create(new GetCategoryTreeQuery(), Error.None)
                .Map(request => new GetCategoryTreeQuery())
                .Bind(command => mediator.Send(command))
                .Match(token => Ok(token), error => resultErrorHandler.Handle(error));
    }
}

