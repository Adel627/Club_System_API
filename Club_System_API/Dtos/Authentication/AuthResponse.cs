using Microsoft.AspNetCore.Identity;

namespace Club_System_API.Dtos.Authentication
{
    public record AuthResponse(
        string Id,
        string? MembershipNumber,
        string FirstName,
        string LastName,
        DateOnly Birth_Of_Date,
        string? ContentType,
        string? Base64Data,
        string PhoneNumber,
        int? MembershipId ,
        string Token,
        int ExpiresIn,
        string RefreshToken,
        DateTime RefreshTokenExpiration
    );
}