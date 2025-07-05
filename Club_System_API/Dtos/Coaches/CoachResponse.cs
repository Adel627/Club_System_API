namespace Club_System_API.Dtos.Coaches
{
    public record CoachResponse(
        int Id,
        string FirstName,
         string LastName,
         string Specialty,
         DateOnly Birth_Of_Date,
         bool IsDisabled,
         string PhoneNumber,
         int Salary,
         double AverageRating,
         string? ContentType,
        string? Base64Data


        );
}
