using SolarLab.AdvertBoard.Domain.Exceptions;

namespace SolarLab.AdvertBoard.Domain.Users
{
    public record Role
    {
        public string Value { get; init; }

        private Role(string value) => Value = value;

        public static Role User => new("User");
        public static Role Admin => new("Admin");

        public static Role FromString(string value) =>
            value.ToLower() switch
            {
                "admin" => Admin,
                "user" => User,
                _ => throw new DomainException($"Unknown role: {value}")
            };

        public static explicit operator string(Role role) => role.Value;
    }
}
