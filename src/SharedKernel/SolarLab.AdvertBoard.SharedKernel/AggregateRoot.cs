namespace SolarLab.AdvertBoard.SharedKernel
{
    /// <summary>
    /// Базовый класс для агрегатов в доменной модели.
    /// </summary>
    public abstract class AggregateRoot : Entity
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AggregateRoot"/>.
        /// </summary>
        protected AggregateRoot() { }
        private readonly List<IDomainEvent> _domainEvents = [];

        /// <summary>
        /// Получает коллекцию доменных событий, произошедших в агрегате.
        /// </summary>
        /// <value>Доступная только для чтения коллекция доменных событий.</value>
        public IReadOnlyCollection<IDomainEvent> DomainEvents => [.. _domainEvents.AsReadOnly()];

        /// <summary>
        /// Очищает коллекцию доменных событий агрегата.
        /// </summary>
        public void ClearDomainEvents() => _domainEvents.Clear();

        /// <summary>
        /// Добавляет доменное событие в коллекцию агрегата.
        /// </summary>
        /// <param name="domainEvent">Доменное событие для добавления.</param>
        public void Raise(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    }
}
