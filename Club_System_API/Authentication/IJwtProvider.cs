using Club_System_API.Models;

namespace Club_System_API.Authentication
{
    public interface IJwtProvider
    {
        (string token, int expiresIn) GenerateToken(ApplicationUser user, IEnumerable<string> roles);
        string? ValidateToken(string token);
    }
}
