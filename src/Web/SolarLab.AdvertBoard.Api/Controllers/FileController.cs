using MediatR;
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
    [ApiController]
    public class FileController(
       IMediator mediator,
       ResultErrorHandler resultErrorHandler) : ControllerBase
    {
        [HttpPost(ApiRoutes.Files.Upload)]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 422)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> UploadImage(Guid advertId, IFormFile file) =>
            await Result.Create(GetBytesAsync(file), Error.None)
                .Map(request => new UploadAdvertImageCommand(advertId, file.FileName, file.ContentType, request.Result))
                .Bind(command => mediator.Send(command))
                .Match(Ok, error => resultErrorHandler.Handle(error));

        [HttpDelete(ApiRoutes.Files.DeleteImage)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 422)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> DeleteImageById(Guid advertId, Guid id) =>
            await mediator.Send(new DeleteImageCommand(advertId, id))
                .Match(NoContent, error => resultErrorHandler.Handle(error));

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

