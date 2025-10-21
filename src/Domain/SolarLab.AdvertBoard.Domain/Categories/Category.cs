using SolarLab.AdvertBoard.SharedKernel;

namespace SolarLab.AdvertBoard.Domain.Categories
{
    /// <summary>
    /// Представляет категорию в системе объявлений.
    /// </summary>
    public class Category : AggregateRoot
    {
        /// <summary>
        /// Идентификатор категории.
        /// </summary>
        public CategoryId Id { get; init; } = null!;

        /// <summary>
        /// Идентификатор родительской категории.
        /// </summary>
        /// <value>Null для корневых категорий.</value>
        public CategoryId? ParentId { get; init; }

        /// <summary>
        /// Название категории.
        /// </summary>
        public CategoryTitle Title { get; init; } = null!;

        private readonly List<Category> _childrens = [];

        /// <summary>
        /// Коллекция дочерних категорий.
        /// </summary>
        public IReadOnlyCollection<Category> Childrens => _childrens.AsReadOnly();

        /// <summary>
        /// Получает значение, указывающее, может ли категория содержать объявления.
        /// </summary>
        /// <value>
        /// <c>true</c> если категория не имеет дочерних элементов (листовая категория);
        /// иначе <c>false</c>.
        /// </value>
        public bool CanHostAdverts => _childrens.Count == 0;

        /// <summary>
        /// Приватный конструктор для EF Core.
        /// </summary>
        private Category() { }

        /// <summary>
        /// Создает корневую категорию (без родителя).
        /// </summary>
        /// <param name="title">Название категории.</param>
        /// <returns>Новая корневая категория.</returns>
        public static Category CreateRoot(CategoryTitle title)
        {
            return new Category
            {
                Id = new CategoryId(Guid.NewGuid()),
                Title = title,
            };
        }

        /// <summary>
        /// Добавляет дочернюю категорию к текущей категории.
        /// </summary>
        /// <param name="title">Название дочерней категории.</param>
        /// <returns>Созданная дочерняя категория.</returns>
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
