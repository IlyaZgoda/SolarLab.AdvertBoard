using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolarLab.AdvertBoard.Domain.Users;

namespace SolarLab.AdvertBoard.Persistence.Configurations
{
    /// <summary>
    /// Конфигурация Entity Framework для сущности User.
    /// </summary>
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.IdentityId)
                .IsRequired()
                .HasMaxLength(450)
                .HasColumnName(nameof(User.IdentityId));

            builder.Property(u => u.Id)
                .HasConversion(
                userId => userId.Id,
                value => new UserId(value));

            builder.Property(u => u.FirstName)
                .HasConversion(
                fn => fn.Value,
                value => FirstName.Create(value).Value)
                .IsRequired()
                .HasMaxLength(FirstName.MaxLength)
                .HasColumnName(nameof(FirstName));

            builder.Property(u => u.LastName)
                .HasConversion(
                ln => ln.Value,
                value => LastName.Create(value).Value)
                .IsRequired()
                .HasMaxLength(LastName.MaxLength)
                .HasColumnName(nameof(LastName));

            builder.Property(u => u.MiddleName)
                .HasConversion(
                mn => mn!.Value,
                value => MiddleName.Create(value).Value)
                .IsRequired(false)
                .HasMaxLength(MiddleName.MaxLength)
                .HasColumnName(nameof(MiddleName));

            builder.Property(u => u.ContactEmail)
                .HasConversion(
                ce => ce.Value,
                value => ContactEmail.Create(value).Value)
                .IsRequired()
                .HasMaxLength(ContactEmail.MaxLength)
                .HasColumnName(nameof(ContactEmail));

            builder.Property(u => u.PhoneNumber)
                .HasConversion(
                p => p!.Value,
                value => PhoneNumber.Create(value).Value)
                .IsRequired(false)
                .HasMaxLength(PhoneNumber.MaxLength)
                .HasColumnName(nameof(PhoneNumber));

            builder.Property(u => u.CreatedAt)
                .IsRequired()
                .HasColumnName(nameof(User.CreatedAt));

            builder.HasIndex(u => u.IdentityId);
        }
    }
}
