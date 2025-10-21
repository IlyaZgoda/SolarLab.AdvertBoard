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
    /// <summary>
    /// Контроллер для управления объявлениями.
    /// </summary>
    /// <param name="mediator">Медиатор для отправки команд и запросов.</param>
    /// <param name="resultErrorHandler">Обработчик ошибок для преобразования в HTTP ответы.</param>
    [Authorize]
    public class AdvertsController(
        IMediator mediator,
        ResultErrorHandler resultErrorHandler) : ControllerBase
    {
        /// <summary>
        /// Создает новый черновик объявления.
        /// </summary>
        /// <param name="createDraftRequest">Данные для создания черновика объявления.</param>
        /// <returns>
        /// 201 Created с идентификатором созданного черновика при успешном создании.
        /// 400 Bad Request при неверных данных или ошибках валидации.
        /// 401 Unauthorized при отсутствии аутентификации.
        /// 404 Not Found если категория не найдена.
        /// 422 Unprocessable Entity при нарушении бизнес-правил (нелистовая категория).
        /// 500 Internal Server Error при внутренней ошибке сервера.
        /// </returns>
        [HttpPost(ApiRoutes.Adverts.CreateDraft)]
        [ProducesResponseType(typeof(AdvertIdResponse), 201)]
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
                .Match(
                    id => Created(nameof(GetDraftAdvertById), new {id}), 
                    error => resultErrorHandler.Handle(error)
                );

        /// <summary>
        /// Обновляет черновик объявления.
        /// </summary>
        /// <param name="id">Идентификатор черновика объявления.</param>
        /// <param name="updateDraftRequest">Данные для обновления черновика.</param>
        /// <returns>
        /// 204 No Content при успешном обновлении.
        /// 400 Bad Request при неверных данных или ошибках валидации.
        /// 401 Unauthorized при отсутствии аутентификации.
        /// 404 Not Found если черновик не найден.
        /// 422 Unprocessable Entity при нарушении бизнес-правил.
        /// 500 Internal Server Error при внутренней ошибке сервера.
        /// </returns>
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

        /// <summary>
        /// Получает черновик объявления по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор черновика объявления.</param>
        /// <returns>
        /// 200 OK с данными черновика при успешном получении.
        /// 401 Unauthorized при отсутствии аутентификации.
        /// 404 Not Found если черновик не найден.
        /// 500 Internal Server Error при внутренней ошибке сервера.
        /// </returns>
        [HttpGet(ApiRoutes.Adverts.GetDraft)]
        [ProducesResponseType(typeof(AdvertDraftDetailsResponse), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> GetDraftAdvertById(Guid id) =>
            await mediator.Send(new GetAdvertDraftByIdQuery(id))
                .Match(response => Ok(response), error => resultErrorHandler.Handle(error));

        /// <summary>
        /// Удаляет черновик объявления.
        /// </summary>
        /// <param name="id">Идентификатор черновика объявления.</param>
        /// <returns>
        /// 204 No Content при успешном удалении.
        /// 401 Unauthorized при отсутствии аутентификации.
        /// 404 Not Found если черновик не найден.
        /// 500 Internal Server Error при внутренней ошибке сервера.
        /// </returns>
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

        /// <summary>
        /// Публикует черновик объявления.
        /// </summary>
        /// <param name="id">Идентификатор черновика объявления.</param>
        /// <returns>
        /// 204 No Content при успешной публикации.
        /// 401 Unauthorized при отсутствии аутентификации.
        /// 404 Not Found если черновик не найден.
        /// 500 Internal Server Error при внутренней ошибке сервера.
        /// </returns>
        [HttpPatch(ApiRoutes.Adverts.PublishDraft)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> PublishDraft(Guid id) =>
            await mediator.Send(new PublishAdvertDraftCommand(id))
                .Match(NoContent, error => resultErrorHandler.Handle(error));

        /// <summary>
        /// Удаляет опубликованное объявление.
        /// </summary>
        /// <param name="id">Идентификатор опубликованного объявления.</param>
        /// <returns>
        /// 204 No Content при успешном удалении.
        /// 401 Unauthorized при отсутствии аутентификации.
        /// 404 Not Found если объявление не найдено.
        /// 500 Internal Server Error при внутренней ошибке сервера.
        /// </returns>
        [HttpDelete(ApiRoutes.Adverts.DeletePublished)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> DeletePublished(Guid id) =>
            await mediator.Send(new DeletePublishedAdvertCommand(id))
                .Match(NoContent, error => resultErrorHandler.Handle(error));

        /// <summary>
        /// Получает опубликованное объявление по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор опубликованного объявления.</param>
        /// <returns>
        /// 200 OK с данными опубликованного объявления при успешном получении.
        /// 401 Unauthorized при отсутствии аутентификации.
        /// 404 Not Found если объявление не найдено.
        /// 500 Internal Server Error при внутренней ошибке сервера.
        /// </returns>
        [AllowAnonymous] 
        [HttpGet(ApiRoutes.Adverts.GetPublished)]
        [ProducesResponseType(typeof(PublishedAdvertDetailsResponse), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> GetPublishedAdvertById(Guid id) =>
            await mediator.Send(new GetPublishedAdvertDetailsByIdQuery(id))
                .Match(response => Ok(response), error => resultErrorHandler.Handle(error));

        /// <summary>
        /// Получает опубликованные объявления по фильтру.
        /// </summary>
        /// <param name="filter">Параметры фильтрации и пагинации.</param>
        /// <returns>
        /// 200 OK с пагинированной коллекцией объявлений при успешном получении.
        /// 401 Unauthorized при отсутствии аутентификации.
        /// 404 Not Found если объявления не найдены.
        /// 500 Internal Server Error при внутренней ошибке сервера.
        /// </returns>
        [AllowAnonymous] 
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

        /// <summary>
        /// Получает черновики объявлений текущего пользователя.
        /// </summary>
        /// <param name="getUserAdvertDraftsRequest">Параметры пагинации.</param>
        /// <returns>
        /// 200 OK с пагинированной коллекцией черновиков при успешном получении.
        /// 401 Unauthorized при отсутствии аутентификации.
        /// 404 Not Found если черновики не найдены.
        /// 500 Internal Server Error при внутренней ошибке сервера.
        /// </returns>
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

        /// <summary>
        /// Получает опубликованные объявления текущего пользователя.
        /// </summary>
        /// <param name="getUserPublishedAdvertsRequest">Параметры пагинации.</param>
        /// <returns>
        /// 200 OK с пагинированной коллекцией опубликованных объявлений при успешном получении.
        /// 401 Unauthorized при отсутствии аутентификации.
        /// 404 Not Found если объявления не найдены.
        /// 500 Internal Server Error при внутренней ошибке сервера.
        /// </returns>
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

