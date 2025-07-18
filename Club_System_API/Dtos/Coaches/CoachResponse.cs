namespace Club_System_API.Dtos.Coaches
{
    public record CoachResponse(
         int Id,
         string FirstName,
         string LastName,
         string Specialty,
         string? Bio,
         double AverageRating,
         string? ContentType,
        string? Base64Data


        );
}
