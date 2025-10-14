using FluentAssertions;
using SolarLab.AdvertBoard.Domain.Users;
using SolarLab.AdvertBoard.Domain.Users.Events;

namespace SolarLab.AdvertBoard.UnitTests.Domain.Users
{
    public class UserTests
    {
        private readonly string _identityId = "auth0|123456789";
        private readonly FirstName _firstName;
        private readonly LastName _lastName;
        private readonly ContactEmail _contactEmail;

        public UserTests()
        {
            _firstName = FirstName.Create("John").Value;
            _lastName = LastName.Create("Doe").Value;
            _contactEmail = ContactEmail.Create("john.doe@example.com").Value;
        }

        [Fact]
        public void Create_Should_Create_User_With_Required_Properties()
        {
            // Act
            var user = User.Create(_identityId, _firstName, _lastName, null, _contactEmail, null);

            // Assert
            user.Should().NotBeNull();
            user.Id.Should().NotBeNull();
            user.IdentityId.Should().Be(_identityId);
            user.FirstName.Should().Be(_firstName);
            user.LastName.Should().Be(_lastName);
            user.ContactEmail.Should().Be(_contactEmail);
            user.MiddleName.Should().BeNull();
            user.PhoneNumber.Should().BeNull();
            user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void Create_Should_Create_User_With_All_Properties()
        {
            // Arrange
            var middleName = MiddleName.Create("Michael").Value;
            var phoneNumber = PhoneNumber.Create("+12345678901").Value;

            // Act
            var user = User.Create(_identityId, _firstName, _lastName, middleName, _contactEmail, phoneNumber);

            // Assert
            user.Should().NotBeNull();
            user.MiddleName.Should().Be(middleName);
            user.PhoneNumber.Should().Be(phoneNumber);
        }

        [Fact]
        public void Create_Should_Raise_UserRegisteredDomainEvent()
        {
            // Act
            var user = User.Create(_identityId, _firstName, _lastName, null, _contactEmail, null);

            // Assert
            var domainEvents = user.DomainEvents.ToList();
            domainEvents.Should().HaveCount(1);

            var domainEvent = domainEvents.First().Should().BeOfType<UserRegisteredDomainEvent>().Subject;
            domainEvent.UserId.Should().Be(user.Id);
            domainEvent.IdentityId.Should().Be(_identityId);
        }

        [Fact]
        public void FullName_Should_Return_Correct_Format_Without_MiddleName()
        {
            // Arrange
            var user = User.Create(_identityId, _firstName, _lastName, null, _contactEmail, null);

            // Act
            var fullName = user.FullName;

            // Assert
            fullName.Should().Be("Doe John ");
        }

        [Fact]
        public void FullName_Should_Return_Correct_Format_With_MiddleName()
        {
            // Arrange
            var middleName = MiddleName.Create("Michael").Value;
            var user = User.Create(_identityId, _firstName, _lastName, middleName, _contactEmail, null);

            // Act
            var fullName = user.FullName;

            // Assert
            fullName.Should().Be("Doe John Michael");
        }

        [Fact]
        public void FullName_Should_Return_Correct_Format_With_Empty_MiddleName()
        {
            // Arrange
            var middleName = MiddleName.Create("").Value; // This returns null
            var user = User.Create(_identityId, _firstName, _lastName, middleName, _contactEmail, null);

            // Act
            var fullName = user.FullName;

            // Assert
            fullName.Should().Be("Doe John ");
        }

        [Fact]
        public void Create_Should_Generate_Unique_UserId()
        {
            // Act
            var user1 = User.Create(_identityId, _firstName, _lastName, null, _contactEmail, null);
            var user2 = User.Create("auth0|987654321", _firstName, _lastName, null, _contactEmail, null);

            // Assert
            user1.Id.Should().NotBe(user2.Id);
        }

        [Fact]
        public void User_Should_Be_Equal_When_Same_Id()
        {
            // Arrange
            var user1 = User.Create(_identityId, _firstName, _lastName, null, _contactEmail, null);
            var user2 = user1; 

            // Act & Assert
            user1.Should().Be(user2);
            user1.Equals(user2).Should().BeTrue();
        }

        [Fact]
        public void User_Should_Not_Be_Equal_When_Different_Instances()
        {
            // Arrange
            var user1 = User.Create(_identityId, _firstName, _lastName, null, _contactEmail, null);
            var user2 = User.Create("auth0|987654321", _firstName, _lastName, null, _contactEmail, null);

            // Act & Assert
            user1.Should().NotBe(user2);
            user1.Equals(user2).Should().BeFalse();
        }
    }
}
