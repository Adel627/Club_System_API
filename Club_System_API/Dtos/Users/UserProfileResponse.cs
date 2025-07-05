namespace Club_System_API.Dtos.Users;

public record UserProfileResponse(
    string MembershipNumber,
    string UserName,
    string FirstName,
    string LastName,
    DateOnly Birth_Of_Date,
    int? MembershipId,
    DateTime? MembershipStartDate,
    DateTime? MembershipEndDate,
    string? ContentType,
        string? Base64Data
    );