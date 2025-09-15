using SolarLab.AdvertBoard.Domain.Users.Events;
using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Domain.Users
{
    public class User : AggregateRoot
    {
        public UserId Id { get; init; } = null!;

        public string IdentityId { get; init; } = null!;

        public FirstName FirstName { get; private set; } = null!;

        public LastName LastName { get; private set; } = null!;

        public MiddleName? MiddleName { get; private set; }

        public PhoneNumber PhoneNumber { get; private set; } = null!;

        public DateTime CreatedAt { get; init; }

        private User() { }

        private User(
            string identityId,
            FirstName firstName,
            LastName lastName,
            MiddleName? middleName,
            PhoneNumber phoneNumber)
        {
            Id = new UserId(Guid.NewGuid());
            IdentityId = identityId;
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            PhoneNumber = phoneNumber;
            CreatedAt = DateTime.UtcNow;

        }

        public static User Create(
            string identityId,
            FirstName firstName, 
            LastName lastName, 
            MiddleName? middleName, 
            PhoneNumber phoneNumber)
        {
            var user = new User(identityId, firstName, lastName, middleName, phoneNumber);

            user.Raise(new UserRegisteredDomainEvent(user.Id, user.IdentityId));

            return user;
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
