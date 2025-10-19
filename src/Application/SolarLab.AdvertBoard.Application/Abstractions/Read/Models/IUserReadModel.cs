namespace SolarLab.AdvertBoard.Application.Abstractions.Read.Models
{
    public interface IUserReadModel
    {
        public Guid Id { get; }
        public string IdentityId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string? MiddleName { get; }
        public string ContactEmail { get; }
        public string? PhoneNumber { get; }
        public DateTime CreatedAt { get; }

        public string FullName => $"{LastName} {FirstName} {MiddleName}";

        public IReadOnlyList<IAdvertReadModel> Adverts { get; }
        public IReadOnlyList<ICommentReadModel> Comments { get; }
    }
}
