using Club_System_API.Models;

namespace Club_System_API.Dtos.CoachRating
{
    public record RateCoachRequest(
          int CoachId ,
          int Rating 
        );
}
