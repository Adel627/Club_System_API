using Microsoft.AspNetCore.Identity;
using Club_System_API.Models; // لو هتحتاج الـ enums
using System.Collections.Generic;

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
        int? MembershipId,
        string Token,
        int ExpiresIn,
        string RefreshToken,
        DateTime RefreshTokenExpiration,

        // ✅ الجديد اللي هنرجعه في الـ response:
        MembershipDetailsDto? MembershipDetails,
        List<ServiceBookingDto>? Services
    );
}
