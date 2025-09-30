using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Domain.Categories
{
    public class Category : AggregateRoot
    {
        public CategoryId Id { get; init; } = null!;

        public CategoryId? ParentId { get; init; }

        public CategoryTitle Title { get; init; } = null!;

        private readonly List<Category> _childrens = [];

        public IReadOnlyCollection<Category> Childrens => _childrens.AsReadOnly();

        public bool CanHostAdverts => _childrens.Count == 0;

        private Category() { }

        public static Category CreateRoot(CategoryTitle title)
        {
            return new Category
            {
                Id = new CategoryId(Guid.NewGuid()),
                Title = title,
            };
        }

        public Category AddChild(CategoryTitle title)
        {
            var child = new Category
            {
                Id = new CategoryId(Guid.NewGuid()),
                ParentId = this.Id,
                Title = title,
            };

            _childrens.Add(child);

            return child;
        }
    }
}
