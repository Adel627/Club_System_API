namespace Club_System_API.Dtos.Users
{
    public record AddingUserResponse(
    string Id,
    string MembershipNumber,
    string FirstName,
    string LastName,
    DateOnly Birth_Of_Date,
    string? ContentType,
    string? Base64Data,
    bool IsDisabled
    
);
}
