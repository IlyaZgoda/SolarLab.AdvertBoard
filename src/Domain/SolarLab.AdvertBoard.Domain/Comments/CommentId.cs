using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Domain.Comments
{
    public record CommentId(Guid Id) : StronglyTypedId(Id);
}
