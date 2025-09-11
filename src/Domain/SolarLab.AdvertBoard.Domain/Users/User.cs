using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel.Result;

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

        private User() { }

        public static User Create(
            PasswordHash passwordHash, 
            FirstName firstName, 
            LastName lastName, 
            MiddleName? middleName, 
            Email email, 
            PhoneNumber phoneNumber, 
            Role role)
        {
            return new()
            {
                Id = new UserId(Guid.NewGuid()),
                PasswordHash = passwordHash,
                FirstName = firstName,
                LastName = lastName,
                MiddleName = middleName,
                Email = email,
                PhoneNumber = phoneNumber,
                CreatedAt = DateTime.UtcNow,
                Role = role
            };

        }
        
        public void UpdateProfile(
            FirstName firstName, 
            LastName lastName, 
            MiddleName? middleName, 
            PhoneNumber phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            PhoneNumber = phoneNumber;
        }

        public Result ChangePassword(PasswordHash passwordHash)
        {
            if(passwordHash == PasswordHash)
            {
                return Result.Failure(UserErrors.CannotChangePassword);
            }

            PasswordHash = passwordHash;

            return Result.Success();
        }     
    }
}
