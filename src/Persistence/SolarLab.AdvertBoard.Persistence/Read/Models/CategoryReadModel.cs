namespace SolarLab.AdvertBoard.Persistence.Read.Models
{
    public class CategoryReadModel
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Title { get; set; } = null!;

        public List<CategoryReadModel> Childrens { get; set; } = new();
    }

}
