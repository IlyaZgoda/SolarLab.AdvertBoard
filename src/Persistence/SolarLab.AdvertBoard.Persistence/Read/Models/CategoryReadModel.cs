using SolarLab.AdvertBoard.Application.Abstractions.Read.Models;

namespace SolarLab.AdvertBoard.Persistence.Read.Models
{
    public class CategoryReadModel : ICategoryReadModel
    {
        public Guid Id { get; }
        public Guid? ParentId { get; }
        public string Title { get; } = null!;

        public List<CategoryReadModel> Childrens { get; } = [];

        IReadOnlyList<ICategoryReadModel> ICategoryReadModel.Childrens => Childrens;
    }
}
