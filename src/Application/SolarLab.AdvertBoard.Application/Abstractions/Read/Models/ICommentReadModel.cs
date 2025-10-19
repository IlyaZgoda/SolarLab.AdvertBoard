namespace SolarLab.AdvertBoard.Application.Abstractions.Read.Models
{
    public interface ICommentReadModel
    {
        public Guid Id { get;  }
        public Guid AdvertId { get;  }
        public IAdvertReadModel Advert { get; }

        public Guid AuthorId { get;  }
        public IUserReadModel Author { get;  }

        public string Text { get; }
        public DateTime CreatedAt { get; }
        public DateTime? UpdatedAt { get; }
    }
}
