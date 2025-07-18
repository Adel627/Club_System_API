using Club_System_API.Dtos.Service;

namespace Club_System_API.Dtos.Users;

public record UserProfileResponse(
    string? MembershipNumber,
    string UserName,
    string FirstName,
    string LastName,
    DateOnly Birth_Of_Date,
    string? MembershipName,
    DateTime? MembershipStartDate,
    DateTime? MembershipEndDate,
    string? ContentType,
    string? Base64Data,
    DateOnly JoinedAt,
    ICollection<string> Services
   

);
