namespace Club_System_API.Dtos.Users;

public record UserProfileResponse(
    string MembershipNumber,
    string UserName,
    string FirstName,
    string LastName,
    DateOnly Birth_Of_Date,
    DateOnly? Renewal_date,
    byte[]? Image
);