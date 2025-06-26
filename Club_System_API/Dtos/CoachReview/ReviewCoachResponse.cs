namespace Club_System_API.Dtos.CoachRating
{
    public record ReviewCoachResponse(
          int CoachId,
          int Rating,
         DateTime RatedAt
        );
}
