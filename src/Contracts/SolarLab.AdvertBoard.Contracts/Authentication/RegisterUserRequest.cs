namespace SolarLab.AdvertBoard.Contracts.Authentication
{
    public record RegisterUserRequest(
        string FirstName, 
        string LastName, 
        string? MiddleName, 
        string Email, 
        string? ContactEmail,
        string Password, 
        string? PhoneNumber);
}
