namespace Club_System_API.Dtos.Authentication
{
    public record MembershipDetailsDto(
        int Id,
        string Name,
        string Description,
        decimal Price,
        DateTime StartDate,
        DateTime EndDate
    );
}
