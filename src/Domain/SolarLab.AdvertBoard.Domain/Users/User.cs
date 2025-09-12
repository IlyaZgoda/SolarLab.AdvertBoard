using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.SharedKernel.Result;

namespace SolarLab.AdvertBoard.Domain.Users
{
    public class User
    {
        public UserId Id { get; init; } = null!;

        public string IdentityId { get; init; } = null!;

        public FirstName FirstName { get; private set; } = null!;

        public LastName LastName { get; private set; } = null!;

        public MiddleName? MiddleName { get; private set; }

        public PhoneNumber PhoneNumber { get; private set; } = null!;

        public DateTime CreatedAt { get; init; }

        private User() { }

        public static User Create(
            FirstName firstName, 
            LastName lastName, 
            MiddleName? middleName, 
            PhoneNumber phoneNumber)
        {
            return new()
            {
                Id = new UserId(Guid.NewGuid()),
                FirstName = firstName,
                LastName = lastName,
                MiddleName = middleName,
                PhoneNumber = phoneNumber,
                CreatedAt = DateTime.UtcNow,
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
    }
}
