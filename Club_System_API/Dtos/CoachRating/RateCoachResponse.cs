namespace Club_System_API.Dtos.CoachRating
{
    public record RateCoachResponse(
          int CoachId,
          int Rating,
         DateTime RatedAt
        );
}
