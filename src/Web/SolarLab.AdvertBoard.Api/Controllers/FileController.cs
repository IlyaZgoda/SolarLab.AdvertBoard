using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarLab.AdvertBoard.Api.Mappers;
using SolarLab.AdvertBoard.Application.Images.Delete;
using SolarLab.AdvertBoard.Application.Images.GetById;
using SolarLab.AdvertBoard.Application.Images.UploadImage;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using SolarLab.AdvertBoard.SharedKernel.Result.Methods.Extensions;

namespace SolarLab.AdvertBoard.Api.Controllers
{
    /// <summary>
    /// Контроллер для управления файлами (изображениями) объявлений.
    /// </summary>
    /// <param name="mediator">Медиатор для отправки команд и запросов.</param>
    /// <param name="resultErrorHandler">Обработчик ошибок для преобразования в HTTP ответы.</param>
    [ApiController]
    public class FileController(
       IMediator mediator,
       ResultErrorHandler resultErrorHandler) : ControllerBase
    {
        /// <summary>
        /// Загружает изображение для указанного объявления.
        /// </summary>
        /// <param name="advertId">Идентификатор объявления, для которого загружается изображение.</param>
        /// <param name="file">Файл изображения для загрузки.</param>
        /// <returns>
        /// 201 Created при успешной загрузке.
        /// 400 Bad Request при неверном формате файла или данных.
        /// 401 Unauthorized при отсутствии аутентификации.
        /// 404 Not Found если объявление не найдено.
        /// 422 Unprocessable Entity при нарушении бизнес-правил (например, превышение лимита изображений).
        /// 500 Internal Server Error при внутренней ошибке сервера.
        /// </returns>
        [Authorize]
        [HttpPost(ApiRoutes.Files.Upload)]
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 422)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> UploadImage(Guid advertId, IFormFile file) =>
            await Result.Create(GetBytesAsync(file), Error.None)
                .Map(request => new UploadAdvertImageCommand(advertId, file.FileName, file.ContentType, request.Result))
                .Bind(command => mediator.Send(command))
                .Match(
                    image => CreatedAtAction(nameof(DownloadImageById), new {image.Id}), 
                    error => resultErrorHandler.Handle(error)
                );

        /// <summary>
        /// Удаляет изображение по идентификатору.
        /// </summary>
        /// <param name="advertId">Идентификатор объявления, к которому привязано изображение.</param>
        /// <param name="id">Идентификатор изображения для удаления.</param>
        /// <returns>
        /// 204 No Content при успешном удалении.
        /// 400 Bad Request при неверных параметрах.
        /// 401 Unauthorized при отсутствии аутентификации.
        /// 404 Not Found если изображение или объявление не найдены.
        /// 500 Internal Server Error при внутренней ошибке сервера.
        /// </returns>
        [Authorize]
        [HttpDelete(ApiRoutes.Files.DeleteImage)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> DeleteImageById(Guid advertId, Guid id) =>
            await mediator.Send(new DeleteImageCommand(advertId, id))
                .Match(NoContent, error => resultErrorHandler.Handle(error));

        /// <summary>
        /// Скачивает изображение по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор изображения.</param>
        /// <returns>
        /// 200 OK с содержимым изображения в теле ответа.
        /// 401 Unauthorized при отсутствии аутентификации.
        /// 404 Not Found если изображение не найдено.
        /// 500 Internal Server Error при внутренней ошибке сервера.
        /// </returns>
        [HttpGet(ApiRoutes.Files.DownloadImage)]
        [Produces("image/jpeg", "image/png")]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> DownloadImageById(Guid id) =>
            await mediator.Send(new GetImageByIdQuery(id))
                .Match(
                    response => File(response.Content, response.ContentType),
                    error => resultErrorHandler.Handle(error));

        /// <summary>
        /// Получает изображение по идентификатору с указанием имени файла.
        /// </summary>
        /// <param name="id">Идентификатор изображения.</param>
        /// <returns>
        /// 200 OK с содержимым изображения и именем файла для скачивания.
        /// 401 Unauthorized при отсутствии аутентификации.
        /// 404 Not Found если изображение не найдено.
        /// 500 Internal Server Error при внутренней ошибке сервера.
        /// </returns>
        [HttpGet(ApiRoutes.Files.GetImage)]
        [Produces("image/jpeg", "image/png")]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> GetImageById(Guid id) =>
            await mediator.Send(new GetImageByIdQuery(id))
                .Match(
                    response => File(response.Content, response.ContentType, response.FileName),
                    error => resultErrorHandler.Handle(error));

        private static async Task<byte[]> GetBytesAsync(IFormFile file)
        {
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            return ms.ToArray();
        }
    }
}

