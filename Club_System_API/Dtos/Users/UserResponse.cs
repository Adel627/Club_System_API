namespace Club_System_API.Dtos.Users;

public record UserResponse(
    string Id,
    string MembershipNumber,
     string PhoneNumber,
     bool  PhoneNumberConfirmed,
    string FirstName,
    string LastName,
    DateOnly Birth_Of_Date,
    DateOnly? Renewal_date,
    byte[]? Image,
    bool IsDisabled,
    IEnumerable<string> Roles
);