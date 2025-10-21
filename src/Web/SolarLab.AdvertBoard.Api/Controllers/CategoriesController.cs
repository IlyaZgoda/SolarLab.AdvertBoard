using MediatR;
using Microsoft.AspNetCore.Mvc;
using SolarLab.AdvertBoard.Api.Mappers;
using SolarLab.AdvertBoard.Application.Categories.GetById;
using SolarLab.AdvertBoard.Application.Categories.GetTree;
using SolarLab.AdvertBoard.Contracts.Categories;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;

namespace SolarLab.AdvertBoard.Api.Controllers
{
    /// <summary>
    /// Контроллер для работы с категориями объявлений.
    /// </summary>
    /// <param name="mediator">Медиатор для отправки запросов.</param>
    /// <param name="resultErrorHandler">Обработчик ошибок для преобразования в HTTP ответы.</param>
    public class CategoriesController(
        IMediator mediator, 
        ResultErrorHandler resultErrorHandler) : ControllerBase
    {
        /// <summary>
        /// Получает информацию о категории по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор категории.</param>
        /// <returns>
        /// 200 OK с данными категории при успешном получении.
        /// 404 Not Found если категория с указанным идентификатором не найдена.
        /// 500 Internal Server Error при внутренней ошибке сервера.
        /// </returns>
        [HttpGet(ApiRoutes.Categories.GetById)]
        [ProducesResponseType(typeof(CategoryResponse), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> GetById(Guid id) =>
            await mediator.Send(new GetCategoryByIdQuery(id))
                .Match(id => Ok(id), error => resultErrorHandler.Handle(error));

        /// <summary>
        /// Получает полное дерево категорий.
        /// </summary>
        /// <returns>
        /// 200 OK с древовидной структурой категорий при успешном получении.
        /// 404 Not Found если категории не найдены (система не инициализирована).
        /// 500 Internal Server Error при внутренней ошибке сервера.
        /// </returns>
        [HttpGet(ApiRoutes.Categories.GetTree)]
        [ProducesResponseType(typeof(CategoryTreeResponse), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> GetTree() =>
            await mediator.Send(new GetCategoryTreeQuery())
                .Match(tree => Ok(tree), error => resultErrorHandler.Handle(error));
    }
}

