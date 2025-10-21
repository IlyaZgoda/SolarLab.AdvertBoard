using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Comments;
using SolarLab.AdvertBoard.Domain.Users;

namespace SolarLab.AdvertBoard.Persistence.Configurations
{
    /// <summary>
    /// Конфигурация Entity Framework для сущности Comment.
    /// </summary>
    internal class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasConversion(
                commentId => commentId.Id,
                value => new CommentId(value));

            builder.Property(c => c.AdvertId)
                .HasConversion(
                advertId => advertId.Id,
                value => new AdvertId(value));

            builder.Property(c => c.AuthorId)
                .HasConversion(
                authorId => authorId.Id,
                value => new UserId(value));

            builder.Property(c => c.Text)
                .HasConversion(
                t => t.Value,
                value => CommentText.Create(value).Value)
                .IsRequired()
                .HasMaxLength(CommentText.MaxLength)
                .HasColumnName("Text");

            builder.Property(c => c.CreatedAt)
                .IsRequired()
                .HasColumnName(nameof(Comment.CreatedAt));

            builder.Property(c => c.UpdatedAt)
                .HasColumnName(nameof(Comment.UpdatedAt));

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne<Advert>()
                .WithMany()
                .HasForeignKey(c => c.AdvertId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
