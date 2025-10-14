using FluentAssertions;
using SolarLab.AdvertBoard.Domain.Categories;

namespace SolarLab.AdvertBoard.UnitTests.Domain.Categories
{
    public class CategoryTests
    {
        private readonly CategoryTitle _title;

        public CategoryTests()
        {
            _title = CategoryTitle.Create("Electronics").Value;
        }

        [Fact]
        public void CreateRoot_Should_Create_Category_With_Correct_Properties()
        {
            // Act
            var category = Category.CreateRoot(_title);

            // Assert
            category.Should().NotBeNull();
            category.Id.Should().NotBeNull();
            category.Title.Should().Be(_title);
            category.ParentId.Should().BeNull();
            category.Childrens.Should().BeEmpty();
            category.CanHostAdverts.Should().BeTrue();
        }

        [Fact]
        public void AddChild_Should_Create_Child_Category_With_Correct_Properties()
        {
            // Arrange
            var parent = Category.CreateRoot(_title);
            var childTitle = CategoryTitle.Create("Smartphones").Value;

            // Act
            var child = parent.AddChild(childTitle);

            // Assert
            child.Should().NotBeNull();
            child.Id.Should().NotBeNull();
            child.Title.Should().Be(childTitle);
            child.ParentId.Should().Be(parent.Id);
            child.Childrens.Should().BeEmpty();
            child.CanHostAdverts.Should().BeTrue();
        }

        [Fact]
        public void AddChild_Should_Add_Child_To_Parent_Childrens_Collection()
        {
            // Arrange
            var parent = Category.CreateRoot(_title);
            var childTitle = CategoryTitle.Create("Smartphones").Value;

            // Act
            var child = parent.AddChild(childTitle);

            // Assert
            parent.Childrens.Should().ContainSingle();
            parent.Childrens.First().Should().Be(child);
        }

        [Fact]
        public void CanHostAdverts_Should_Be_True_When_No_Children()
        {
            // Arrange
            var category = Category.CreateRoot(_title);

            // Act & Assert
            category.CanHostAdverts.Should().BeTrue();
        }

        [Fact]
        public void CanHostAdverts_Should_Be_False_When_Has_Children()
        {
            // Arrange
            var parent = Category.CreateRoot(_title);
            var childTitle = CategoryTitle.Create("Smartphones").Value;

            // Act
            parent.AddChild(childTitle);

            // Assert
            parent.CanHostAdverts.Should().BeFalse();
        }

        [Fact]
        public void AddChild_Should_Create_Nested_Hierarchy()
        {
            // Arrange
            var electronics = Category.CreateRoot(CategoryTitle.Create("Electronics").Value);
            var smartphones = electronics.AddChild(CategoryTitle.Create("Smartphones").Value);
            var iPhones = smartphones.AddChild(CategoryTitle.Create("iPhones").Value);

            // Assert
            electronics.ParentId.Should().BeNull();
            electronics.Childrens.Should().ContainSingle().Which.Should().Be(smartphones);

            smartphones.ParentId.Should().Be(electronics.Id);
            smartphones.Childrens.Should().ContainSingle().Which.Should().Be(iPhones);

            iPhones.ParentId.Should().Be(smartphones.Id);
            iPhones.Childrens.Should().BeEmpty();
        }

        [Fact]
        public void CanHostAdverts_Should_Work_Correctly_In_Nested_Hierarchy()
        {
            // Arrange
            var electronics = Category.CreateRoot(CategoryTitle.Create("Electronics").Value);
            var smartphones = electronics.AddChild(CategoryTitle.Create("Smartphones").Value);
            var iPhones = smartphones.AddChild(CategoryTitle.Create("iPhones").Value);

            // Assert
            electronics.CanHostAdverts.Should().BeFalse(); 
            smartphones.CanHostAdverts.Should().BeFalse(); 
            iPhones.CanHostAdverts.Should().BeTrue(); 
        }

        [Fact]
        public void AddChild_Should_Allow_Multiple_Children_For_Same_Parent()
        {
            // Arrange
            var electronics = Category.CreateRoot(CategoryTitle.Create("Electronics").Value);
            var smartphones = CategoryTitle.Create("Smartphones").Value;
            var laptops = CategoryTitle.Create("Laptops").Value;
            var tablets = CategoryTitle.Create("Tablets").Value;

            // Act
            var child1 = electronics.AddChild(smartphones);
            var child2 = electronics.AddChild(laptops);
            var child3 = electronics.AddChild(tablets);

            // Assert
            electronics.Childrens.Should().HaveCount(3);
            electronics.Childrens.Should().Contain(new[] { child1, child2, child3 });
        }

        [Fact]
        public void AddChild_Should_Create_Independent_Children()
        {
            // Arrange
            var parent1 = Category.CreateRoot(CategoryTitle.Create("Electronics").Value);
            var parent2 = Category.CreateRoot(CategoryTitle.Create("Books").Value);
            var childTitle = CategoryTitle.Create("Smartphones").Value;

            // Act
            var child1 = parent1.AddChild(childTitle);
            var child2 = parent2.AddChild(childTitle);

            // Assert
            child1.Should().NotBe(child2);
            child1.Id.Should().NotBe(child2.Id);
            child1.ParentId.Should().Be(parent1.Id);
            child2.ParentId.Should().Be(parent2.Id);
        }

        [Fact]
        public void Childrens_Collection_Should_Be_Immutable_From_Outside()
        {
            // Arrange
            var parent = Category.CreateRoot(_title);
            var childTitle = CategoryTitle.Create("Smartphones").Value;
            var child = parent.AddChild(childTitle);

            // Act & Assert 
            var childrens = parent.Childrens;

            childrens.Should().NotBeNull();
        }

        [Fact]
        public void CreateRoot_Should_Work_With_Different_Titles()
        {
            // Test various category titles
            var testTitles = new[]
            {
                CategoryTitle.Create("Electronics").Value,
                CategoryTitle.Create("Home & Garden").Value,
                CategoryTitle.Create("Real Estate").Value,
                CategoryTitle.Create("Automotive").Value,
                CategoryTitle.Create("Jobs").Value
            };

            foreach (var title in testTitles)
            {
                // Act
                var category = Category.CreateRoot(title);

                // Assert
                category.Title.Should().Be(title);
                category.ParentId.Should().BeNull();
                category.Childrens.Should().BeEmpty();
            }
        }
    }
}
