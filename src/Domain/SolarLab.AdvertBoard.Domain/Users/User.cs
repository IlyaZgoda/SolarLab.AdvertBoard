namespace SolarLab.AdvertBoard.Domain.Users
{
    public class User
    {
        public UserId Id { get; init; } = null!;

        public PasswordHash PasswordHash { get; private set; } = null!;
        
        public FirstName FirstName { get; private set; } = null!;

        public LastName LastName { get; private set; } = null!;

        public MiddleName? MiddleName { get; private set; }

        public Email Email { get; private set; } = null!;

        public PhoneNumber PhoneNumber { get; private set; } = null!;

        public DateTime CreatedAt { get; init; }

        public Role Role { get; private set; } = null!;
    }
}
