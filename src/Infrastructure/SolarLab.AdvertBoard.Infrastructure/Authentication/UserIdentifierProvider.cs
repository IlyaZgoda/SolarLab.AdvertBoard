using Microsoft.AspNetCore.Http;
using SolarLab.AdvertBoard.Application.Abstractions.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SolarLab.AdvertBoard.Infrastructure.Authentication
{
    public class UserIdentifierProvider(IHttpContextAccessor accessor) 
        : IUserIdentifierProvider
    {
        public string IdentityUserId => 
            accessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value 
            ?? throw new UnauthorizedAccessException("User is not authenticated");
    }
}
