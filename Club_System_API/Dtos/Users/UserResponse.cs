namespace Club_System_API.Dtos.Users;

public record UserResponse(
    string Id,
    string MembershipNumber,
    string PhoneNumber,
    string FirstName,
    string LastName,
    DateOnly Birth_Of_Date,
    byte[]? Image,
    int? MembershipId,
    bool IsDisabled,
    IEnumerable<string> Roles
);