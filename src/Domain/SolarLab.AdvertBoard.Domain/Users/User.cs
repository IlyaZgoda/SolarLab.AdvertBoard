using SolarLab.AdvertBoard.Domain.Users.Events;
using SolarLab.AdvertBoard.SharedKernel;
using SolarLab.AdvertBoard.SharedKernel.Result;
using System.Text.RegularExpressions;

namespace SolarLab.AdvertBoard.Domain.Users
{
    public class User : AggregateRoot
    {
        public UserId Id { get; init; } = null!;

        public string IdentityId { get; init; } = null!;

        public FirstName FirstName { get; private set; } = null!;

        public LastName LastName { get; private set; } = null!;

        public MiddleName? MiddleName { get; private set; }

        public ContactEmail ContactEmail { get; private set; } = null!;

        public PhoneNumber? PhoneNumber { get; private set; }

        public DateTime CreatedAt { get; init; }

        private User() { }

        private User(
            string identityId,
            FirstName firstName,
            LastName lastName,
            MiddleName? middleName,
            ContactEmail contactEmail,
            PhoneNumber? phoneNumber)
        {
            Id = new UserId(Guid.NewGuid());
            IdentityId = identityId;
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            ContactEmail = contactEmail;
            PhoneNumber = phoneNumber;
            CreatedAt = DateTime.UtcNow;

        }

        public static User Create(
            string identityId,
            FirstName firstName, 
            LastName lastName, 
            MiddleName? middleName, 
            ContactEmail contactEmail,
            PhoneNumber? phoneNumber)
        {
            var user = new User(identityId, firstName, lastName, middleName, contactEmail, phoneNumber);

            user.Raise(new UserRegisteredDomainEvent(user.Id, user.IdentityId));

            return user;
        }
        
        public void UpdateProfile(
            FirstName firstName, 
            LastName lastName, 
            MiddleName? middleName, 
            ContactEmail contactEmail,
            PhoneNumber? phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            ContactEmail = contactEmail;
            PhoneNumber = phoneNumber;
        }
    }
}
