using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolarLab.AdvertBoard.Domain.Categories;

namespace SolarLab.AdvertBoard.Persistence.Configurations
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasConversion(
                categoryId => categoryId.Id,
                value => new CategoryId(value));

            builder.Property(c => c.ParentId)
                .HasConversion(
                    categoryId => categoryId == null 
                    ? (Guid?)null 
                    : categoryId.Id,  
                    value => value.HasValue 
                    ? new CategoryId(value.Value) 
                    : null) 
                .IsRequired(false); 

            builder.Property(c => c.Title)
                .HasConversion(
                c => c.Value,
                value => CategoryTitle.Create(value).Value)
                .IsRequired()
                .HasMaxLength(CategoryTitle.MaxLength)
                .HasColumnName("Title");

            builder.HasOne<Category>()
                .WithMany(c => c.Childrens)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Metadata
                  .FindNavigation(nameof(Category.Childrens))!
                  .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
