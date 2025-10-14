using FluentAssertions;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Comments;
using SolarLab.AdvertBoard.Domain.Users;

namespace SolarLab.AdvertBoard.UnitTests.Domain.Comments
{
    public class CommentTests
    {
        private readonly AdvertId _advertId;
        private readonly UserId _authorId;
        private readonly CommentText _text;

        public CommentTests()
        {
            _advertId = new AdvertId(Guid.NewGuid());
            _authorId = new UserId(Guid.NewGuid());
            _text = CommentText.Create("This is a test comment").Value;
        }

        [Fact]
        public void Create_Should_Create_Comment_With_Correct_Properties()
        {
            // Act
            var comment = Comment.Create(_advertId, _authorId, _text);

            // Assert
            comment.Should().NotBeNull();
            comment.Id.Should().NotBeNull();
            comment.AdvertId.Should().Be(_advertId);
            comment.AuthorId.Should().Be(_authorId);
            comment.Text.Should().Be(_text);
            comment.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            comment.UpdatedAt.Should().BeNull();
        }

        [Fact]
        public void Create_Should_Generate_Unique_Id()
        {
            // Act
            var comment1 = Comment.Create(_advertId, _authorId, _text);
            var comment2 = Comment.Create(_advertId, _authorId, _text);

            // Assert
            comment1.Id.Should().NotBe(comment2.Id);
        }

        [Fact]
        public void Update_Should_Change_Text_And_Update_Timestamp()
        {
            // Arrange
            var comment = Comment.Create(_advertId, _authorId, _text);
            var newText = CommentText.Create("Updated comment text").Value;

            // Act
            comment.Update(newText);

            // Assert
            comment.Text.Should().Be(newText);
            comment.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void Update_Should_Not_Change_Immutable_Properties()
        {
            // Arrange
            var comment = Comment.Create(_advertId, _authorId, _text);
            var originalId = comment.Id;
            var originalAdvertId = comment.AdvertId;
            var originalAuthorId = comment.AuthorId;
            var originalCreatedAt = comment.CreatedAt;

            var newText = CommentText.Create("Updated text").Value;

            // Act
            comment.Update(newText);

            // Assert
            comment.Id.Should().Be(originalId);
            comment.AdvertId.Should().Be(originalAdvertId);
            comment.AuthorId.Should().Be(originalAuthorId);
            comment.CreatedAt.Should().Be(originalCreatedAt);
        }

        [Fact]
        public void Update_Should_Set_UpdatedAt_On_First_Update()
        {
            // Arrange
            var comment = Comment.Create(_advertId, _authorId, _text);
            var newText = CommentText.Create("First update").Value;

            // Act
            comment.Update(newText);

            // Assert
            comment.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void Update_Should_Update_UpdatedAt_On_Subsequent_Updates()
        {
            // Arrange
            var comment = Comment.Create(_advertId, _authorId, _text);
            var firstText = CommentText.Create("First update").Value;
            var secondText = CommentText.Create("Second update").Value;

            // Act 
            comment.Update(firstText);
            var firstUpdateTime = comment.UpdatedAt;

            Thread.Sleep(10);

            // Act 
            comment.Update(secondText);

            // Assert
            comment.UpdatedAt.Should().BeAfter(firstUpdateTime.Value);
            comment.Text.Should().Be(secondText);
        }


        [Fact]
        public void Create_Should_Work_With_Different_Comment_Texts()
        {
            // Test various comment scenarios
            var testCases = new[]
            {
                CommentText.Create("Short").Value,
                CommentText.Create("Normal comment text").Value,
                CommentText.Create("Comment with emoji 👍").Value,
                CommentText.Create("Comment with special characters: $100 & free shipping!").Value,
                CommentText.Create(new string('a', CommentText.MaxLength)).Value 
            };

            foreach (var testText in testCases)
            {
                // Act
                var comment = Comment.Create(_advertId, _authorId, testText);

                // Assert
                comment.Text.Should().Be(testText);
                comment.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            }
        }

        [Fact]
        public void Update_Should_Work_With_Different_Comment_Texts()
        {
            // Arrange
            var comment = Comment.Create(_advertId, _authorId, _text);
            var testTexts = new[]
            {
                CommentText.Create("Updated short").Value,
                CommentText.Create("Updated normal comment").Value,
                CommentText.Create("Updated with emoji 🚀").Value
            };

            foreach (var testText in testTexts)
            {
                // Act
                comment.Update(testText);

                // Assert
                comment.Text.Should().Be(testText);
                comment.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            }
        }

        [Fact]
        public void Comment_Should_Have_Correct_Initial_State()
        {
            // Act
            var comment = Comment.Create(_advertId, _authorId, _text);

            // Assert
            comment.Id.Should().NotBeNull();
            comment.AdvertId.Should().Be(_advertId);
            comment.AuthorId.Should().Be(_authorId);
            comment.Text.Should().Be(_text);
            comment.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            comment.UpdatedAt.Should().BeNull();
        }

        [Fact]
        public void Update_Should_Preserve_Original_Text_Until_Update()
        {
            // Arrange
            var originalText = CommentText.Create("Original comment").Value;
            var comment = Comment.Create(_advertId, _authorId, originalText);

            // Act & Assert 
            comment.Text.Should().Be(originalText);

            // Act & Assert 
            var newText = CommentText.Create("New comment").Value;
            comment.Update(newText);
            comment.Text.Should().Be(newText);
        }
    }
}
