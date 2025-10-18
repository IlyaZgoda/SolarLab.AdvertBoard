namespace SolarLab.AdvertBoard.Persistence.Read.Models
{
    public class UserReadModel
    {
        public Guid Id { get; set; }
        public string IdentityId { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string ContactEmail { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }

        public string FullName => $"{LastName} {FirstName} {MiddleName}";

        public List<AdvertReadModel> Adverts { get; set; } = new();
        public List<CommentReadModel> Comments { get; set; } = new();
    }
}
