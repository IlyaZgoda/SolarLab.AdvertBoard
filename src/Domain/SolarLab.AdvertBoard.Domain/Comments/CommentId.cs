using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Domain.Comments
{
    /// <summary>
    /// Идентификатор комментария как строго типизированный идентификатор.
    /// </summary>
    /// <param name="Id">Значение идентификатора.</param>
    public record CommentId(Guid Id) : StronglyTypedId(Id);
}
