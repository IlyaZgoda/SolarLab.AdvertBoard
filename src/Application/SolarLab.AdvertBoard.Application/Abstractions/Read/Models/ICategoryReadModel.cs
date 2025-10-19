namespace SolarLab.AdvertBoard.Application.Abstractions.Read.Models
{
    public interface ICategoryReadModel
    {
        public Guid Id { get; }
        public Guid? ParentId { get; }
        public string Title { get; }

        public IReadOnlyList<ICategoryReadModel> Childrens { get; }
    }
}
