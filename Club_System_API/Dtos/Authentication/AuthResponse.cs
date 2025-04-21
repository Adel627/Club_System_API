using Microsoft.AspNetCore.Identity;

namespace Club_System_API.Dtos.Authentication
{
    public record AuthResponse(
        string Id,
        string? MembershipNumber,
        string FirstName,
        string LastName,
        DateOnly Birth_Of_Date,
        byte[]? Image,
        string PhoneNumber,
        DateOnly? Renewal_date,
        string Token,
        int ExpiresIn,
        string RefreshToken,
        DateTime RefreshTokenExpiration
    );
}