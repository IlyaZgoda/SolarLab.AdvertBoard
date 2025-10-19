using SolarLab.AdvertBoard.Application.Abstractions.Read.Models;

namespace SolarLab.AdvertBoard.Persistence.Read.Models
{
    public class UserReadModel : IUserReadModel
    {
        public Guid Id { get; }
        public string IdentityId { get; } = null!;
        public string FirstName { get; } = null!;
        public string LastName { get; } = null!;
        public string? MiddleName { get; }
        public string ContactEmail { get; } = null!;
        public string? PhoneNumber { get; }
        public DateTime CreatedAt { get; }

        public string FullName => $"{LastName} {FirstName} {MiddleName}";

        public List<AdvertReadModel> Adverts { get; } = [];
        public List<CommentReadModel> Comments { get; } = [];

        IReadOnlyList<IAdvertReadModel> IUserReadModel.Adverts => Adverts;

        IReadOnlyList<ICommentReadModel> IUserReadModel.Comments => Comments;
    }
}
