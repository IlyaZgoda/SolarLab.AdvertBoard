namespace SolarLab.AdvertBoard.Domain.Adverts
{
    /// <summary>
    /// Статусы объявления в системе.
    /// </summary>
    public enum AdvertStatus
    {
        /// <summary>
        /// Черновик - объявление находится в процессе редактирования.
        /// </summary>
        Draft = 0,

        /// <summary>
        /// Опубликовано - объявление доступно для просмотра.
        /// </summary>
        Published = 1,
    }
}
