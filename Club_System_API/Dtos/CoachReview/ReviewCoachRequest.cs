using Club_System_API.Models;

namespace Club_System_API.Dtos.CoachRating
{
    public record ReviewCoachRequest(
          int CoachId ,
          string Review,
          int Rating
         
      
        );
}
