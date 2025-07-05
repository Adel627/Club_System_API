namespace Club_System_API.Dtos.Users;

public record UserResponse(
    string Id,
    string MembershipNumber,
    string PhoneNumber,
    string FirstName,
    string LastName,
    DateOnly Birth_Of_Date,
    int? MembershipId,
    string? ContentType,
    string? Base64Data,
    bool IsDisabled,
    IEnumerable<string> Roles
);