namespace SolarLab.AdvertBoard.Contracts.Users
{
    /// <summary>
    /// DTO для получения идентификатора пользователя.
    /// </summary>
    /// <param name="UserId">Идентификатор пользователя.</param>
    public record UserIdResponse(Guid UserId);  
}
