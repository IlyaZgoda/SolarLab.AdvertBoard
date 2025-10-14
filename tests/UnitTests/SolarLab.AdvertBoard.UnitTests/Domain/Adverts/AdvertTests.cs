using FluentAssertions;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Adverts.Events;
using SolarLab.AdvertBoard.Domain.Categories;
using SolarLab.AdvertBoard.Domain.Errors;
using SolarLab.AdvertBoard.Domain.Exceptions;
using SolarLab.AdvertBoard.Domain.Users;

namespace SolarLab.AdvertBoard.UnitTests.Domain.Adverts
{
    public class AdvertTests
    {
        private readonly UserId _authorId;
        private readonly CategoryId _categoryId;
        private readonly AdvertTitle _title;
        private readonly AdvertDescription _description;
        private readonly Price _price;

        public AdvertTests()
        {
            _authorId = new UserId(Guid.NewGuid());
            _categoryId = new CategoryId(Guid.NewGuid());
            _title = AdvertTitle.Create("MacBook Pro 2023").Value;
            _description = AdvertDescription.Create("Excellent condition, used for 2 months").Value;
            _price = Price.Create(1500.50m).Value;
        }

        [Fact]
        public void CreateDraft_Should_Create_Advert_With_Correct_Properties()
        {
            // Act
            var advert = Advert.CreateDraft(_authorId, _categoryId, _title, _description, _price);

            // Assert
            advert.Should().NotBeNull();
            advert.Id.Should().NotBeNull();
            advert.AuthorId.Should().Be(_authorId);
            advert.CategoryId.Should().Be(_categoryId);
            advert.Title.Should().Be(_title);
            advert.Description.Should().Be(_description);
            advert.Price.Should().Be(_price);
            advert.Status.Should().Be(AdvertStatus.Draft);
            advert.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            advert.PublishedAt.Should().BeNull();
            advert.UpdatedAt.Should().BeNull();
        }

        [Fact]
        public void CreateDraft_Should_Generate_Unique_Id()
        {
            // Act
            var advert1 = Advert.CreateDraft(_authorId, _categoryId, _title, _description, _price);
            var advert2 = Advert.CreateDraft(_authorId, _categoryId, _title, _description, _price);

            // Assert
            advert1.Id.Should().NotBe(advert2.Id);
        }

        [Fact]
        public void Publish_Should_Change_Status_To_Published_For_Draft()
        {
            // Arrange
            var advert = Advert.CreateDraft(_authorId, _categoryId, _title, _description, _price);

            // Act
            advert.Publish();

            // Assert
            advert.Status.Should().Be(AdvertStatus.Published);
            advert.PublishedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            advert.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void Publish_Should_Throw_When_Advert_Is_Not_Draft()
        {
            // Arrange
            var advert = Advert.CreateDraft(_authorId, _categoryId, _title, _description, _price);
            advert.Publish(); // Now it's published

            // Act
            Action act = () => advert.Publish();

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage(AdvertErrors.CantPublishNonDraftAdvert.Description);
        }

        [Fact]
        public void Unpublish_Should_Raise_DomainEvent_For_Published_Advert()
        {
            // Arrange
            var advert = Advert.CreateDraft(_authorId, _categoryId, _title, _description, _price);
            advert.Publish();

            // Act
            advert.Unpublish();

            // Assert
            var domainEvents = advert.DomainEvents.ToList();
            domainEvents.Should().HaveCount(1);

            var domainEvent = domainEvents.First().Should().BeOfType<PublishedAdvertDeletedDomainEvent>().Subject;
            domainEvent.Id.Should().Be(advert.Id);
        }

        [Fact]
        public void Unpublish_Should_Throw_When_Advert_Is_Not_Published()
        {
            // Arrange
            var advert = Advert.CreateDraft(_authorId, _categoryId, _title, _description, _price);
            // Still draft, not published

            // Act
            Action act = () => advert.Unpublish();

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage(AdvertErrors.CanOnlyUnpublishPublishedAdverts.Description);
        }

        [Fact]
        public void DeleteDraft_Should_Not_Throw_For_Draft_Advert()
        {
            // Arrange
            var advert = Advert.CreateDraft(_authorId, _categoryId, _title, _description, _price);

            // Act
            Action act = () => advert.DeleteDraft();

            // Assert 
            act.Should().NotThrow();
        }

        [Fact]
        public void DeleteDraft_Should_Throw_When_Advert_Is_Not_Draft()
        {
            // Arrange
            var advert = Advert.CreateDraft(_authorId, _categoryId, _title, _description, _price);
            advert.Publish(); // Now it's published

            // Act
            Action act = () => advert.DeleteDraft();

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage(AdvertErrors.CanOnlyUnpublishPublishedAdverts.Description);
        }

        [Fact]
        public void DeleteDraft_Should_Not_Raise_DomainEvents()
        {
            // Arrange
            var advert = Advert.CreateDraft(_authorId, _categoryId, _title, _description, _price);

            // Act
            advert.DeleteDraft();

            // Assert
            advert.DomainEvents.Should().BeEmpty();
        }

        [Fact]
        public void UpdateDraft_Should_Update_All_Properties()
        {
            // Arrange
            var advert = Advert.CreateDraft(_authorId, _categoryId, _title, _description, _price);
            var newCategoryId = new CategoryId(Guid.NewGuid());
            var newTitle = AdvertTitle.Create("iPhone 15 Pro").Value;
            var newDescription = AdvertDescription.Create("Brand new, sealed box").Value;
            var newPrice = Price.Create(1200.00m).Value;

            // Act
            advert.UpdateDraft(newCategoryId, newTitle, newDescription, newPrice);

            // Assert
            advert.CategoryId.Should().Be(newCategoryId);
            advert.Title.Should().Be(newTitle);
            advert.Description.Should().Be(newDescription);
            advert.Price.Should().Be(newPrice);
            advert.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void UpdateDraft_Should_Update_Partial_Properties()
        {
            // Arrange
            var advert = Advert.CreateDraft(_authorId, _categoryId, _title, _description, _price);
            var newTitle = AdvertTitle.Create("Updated Title").Value;
            var newPrice = Price.Create(999.99m).Value;

            // Act
            advert.UpdateDraft(null, newTitle, null, newPrice);

            // Assert
            advert.CategoryId.Should().Be(_categoryId); 
            advert.Title.Should().Be(newTitle); 
            advert.Description.Should().Be(_description); 
            advert.Price.Should().Be(newPrice); 
            advert.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void UpdateDraft_Should_Throw_When_Advert_Is_Not_Draft()
        {
            // Arrange
            var advert = Advert.CreateDraft(_authorId, _categoryId, _title, _description, _price);
            advert.Publish(); 

            var newTitle = AdvertTitle.Create("New Title").Value;

            // Act
            Action act = () => advert.UpdateDraft(null, newTitle, null, null);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage(AdvertErrors.CantUpdateNonDraftAdvert.Description);
        }

        [Fact]
        public void UpdateDraft_Should_Throw_When_No_Changes_Provided()
        {
            // Arrange
            var advert = Advert.CreateDraft(_authorId, _categoryId, _title, _description, _price);

            // Act
            Action act = () => advert.UpdateDraft(null, null, null, null);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage(AdvertErrors.NoChanges.Description);
        }

        [Fact]
        public void UpdateDraft_Should_Throw_When_No_Actual_Changes()
        {
            // Arrange
            var advert = Advert.CreateDraft(_authorId, _categoryId, _title, _description, _price);

            // Act
            Action act = () => advert.UpdateDraft(_categoryId, _title, _description, _price);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage(AdvertErrors.NoChanges.Description);
        }

        [Fact]
        public void UpdateDraft_Should_Update_Single_Property()
        {
            var testCases = new[]
            {
                new {
                    Property = "Category",
                    UpdateAction = new Action<Advert>(a => a.UpdateDraft(new CategoryId(Guid.NewGuid()), null, null, null))
                },
                new {
                    Property = "Title",
                    UpdateAction = new Action<Advert>(a => a.UpdateDraft(null, AdvertTitle.Create("New Title").Value, null, null))
                },
                new {
                    Property = "Description",
                    UpdateAction = new Action<Advert>(a => a.UpdateDraft(null, null, AdvertDescription.Create("New Description").Value, null))
                },
                new {
                    Property = "Price",
                    UpdateAction = new Action<Advert>(a => a.UpdateDraft(null, null, null, Price.Create(999.99m).Value))
                }
            };

            foreach (var testCase in testCases)
            {
                // Arrange
                var advert = Advert.CreateDraft(_authorId, _categoryId, _title, _description, _price);

                // Act & Assert
                testCase.UpdateAction(advert);
                advert.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            }
        }

        [Fact]
        public void Advert_Should_Be_Immutable_For_Critical_Properties()
        {
            // Arrange
            var advert = Advert.CreateDraft(_authorId, _categoryId, _title, _description, _price);
            var originalId = advert.Id;
            var originalAuthorId = advert.AuthorId;
            var originalCreatedAt = advert.CreatedAt;

            // Act 
            var newTitle = AdvertTitle.Create("New Title").Value;
            advert.UpdateDraft(null, newTitle, null, null);

            // Assert 
            advert.Id.Should().Be(originalId);
            advert.AuthorId.Should().Be(originalAuthorId);
            advert.CreatedAt.Should().Be(originalCreatedAt);
        }

        [Fact]
        public void Publish_Should_Not_Affect_Immutable_Properties()
        {
            // Arrange
            var advert = Advert.CreateDraft(_authorId, _categoryId, _title, _description, _price);
            var originalId = advert.Id;
            var originalAuthorId = advert.AuthorId;
            var originalCreatedAt = advert.CreatedAt;

            // Act
            advert.Publish();

            // Assert
            advert.Id.Should().Be(originalId);
            advert.AuthorId.Should().Be(originalAuthorId);
            advert.CreatedAt.Should().Be(originalCreatedAt);
        }


        [Fact]
        public void Status_Transitions_Should_Work_Correctly()
        {
            // Arrange
            var advert = Advert.CreateDraft(_authorId, _categoryId, _title, _description, _price);

            // Act & Assert
            advert.Status.Should().Be(AdvertStatus.Draft);
            advert.Publish();
            advert.Status.Should().Be(AdvertStatus.Published);

            // Act & Assert 
            advert.Unpublish();
            advert.Status.Should().Be(AdvertStatus.Published); 
        }
    }
}
