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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SolarLab.AdvertBoard.Api.Controllers
{
    /// <summary>
    /// Контроллер для управления комментариями к объявлениям.
    /// </summary>
    /// <param name="mediator">Медиатор для отправки команд и запросов.</param>
    /// <param name="resultErrorHandler">Обработчик ошибок для преобразования в HTTP ответы.</param>
    [Authorize]
    public class CommentsController(
        IMediator mediator,
        ResultErrorHandler resultErrorHandler) : ControllerBase
    {
        /// <summary>
        /// Создает новый комментарий к объявлению.
        /// </summary>
        /// <param name="advertId">Идентификатор объявления, к которому добавляется комментарий.</param>
        /// <param name="createCommentRequest">Данные для создания комментария.</param>
        /// <returns>
        /// 200 OK с идентификатором созданного комментария при успешном создании.
        /// 400 Bad Request при неверных данных или ошибках валидации.
        /// 401 Unauthorized при отсутствии аутентификации.
        /// 404 Not Found если объявление не найдено.
        /// 422 Unprocessable Entity при нарушении бизнес-правил.
        /// 500 Internal Server Error при внутренней ошибке сервера.
        /// </returns>
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

        /// <summary>
        /// Обновляет существующий комментарий.
        /// </summary>
        /// <param name="commentId">Идентификатор комментария для обновления.</param>
        /// <param name="updateCommentRequest">Новые данные комментария.</param>
        /// <returns>
        /// 204 No Content при успешном обновлении.
        /// 400 Bad Request при неверных данных или ошибках валидации.
        /// 401 Unauthorized при отсутствии аутентификации.
        /// 404 Not Found если комментарий не найден.
        /// 422 Unprocessable Entity при нарушении бизнес-правил.
        /// 500 Internal Server Error при внутренней ошибке сервера.
        /// </returns>
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

        /// <summary>
        /// Удаляет комментарий.
        /// </summary>
        /// <param name="commentId">Идентификатор комментария для удаления.</param>
        /// <returns>
        /// 204 No Content при успешном удалении.
        /// 401 Unauthorized при отсутствии аутентификации.
        /// 404 Not Found если комментарий не найден.
        /// 500 Internal Server Error при внутренней ошибке сервера.
        /// </returns>
        [HttpDelete(ApiRoutes.Comments.Delete)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> Delete(Guid commentId) =>
            await mediator.Send(new DeleteCommentCommand(commentId))
                .Match(NoContent, error => resultErrorHandler.Handle(error));

        /// <summary>
        /// Получает комментарий по идентификатору.
        /// </summary>
        /// <param name="commentId">Идентификатор комментария.</param>
        /// <returns>
        /// 200 OK с данными комментария при успешном получении.
        /// 401 Unauthorized при отсутствии аутентификации.
        /// 404 Not Found если комментарий не найден.
        /// 500 Internal Server Error при внутренней ошибке сервера.
        /// </returns>
        [AllowAnonymous]
        [HttpGet(ApiRoutes.Comments.GetById)]
        [ProducesResponseType(typeof(CommentResponse), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> GetCommentById(Guid commentId) =>
            await mediator.Send(new GetCommentByIdQuery(commentId))
                .Match(response => Ok(response), error => resultErrorHandler.Handle(error));

        /// <summary>
        /// Получает пагинированный список комментариев для объявления.
        /// </summary>
        /// <param name="advertId">Идентификатор объявления.</param>
        /// <param name="getCommentsByAdvertIdRequest">Параметры пагинации.</param>
        /// <returns>
        /// 200 OK с пагинированной коллекцией комментариев при успешном получении.
        /// 401 Unauthorized при отсутствии аутентификации.
        /// 404 Not Found если объявление не найдено.
        /// 500 Internal Server Error при внутренней ошибке сервера.
        /// </returns>
        [AllowAnonymous]
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

