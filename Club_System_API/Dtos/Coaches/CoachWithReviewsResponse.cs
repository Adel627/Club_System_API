using Club_System_API.Dtos.Appointment;
using Club_System_API.Dtos.CoachRating;
using Club_System_API.Dtos.CoachReview;

namespace Club_System_API.Dtos.Coaches
{
    public record CoachWithReviewsResponse(
         int Id,
         string FirstName,
         string LastName,
         string PhoneNumber,
         string Specialty,
         string? Bio,
         string? Description,
         double AverageRating,
         string? ContentType,
        string? Base64Data,
        ICollection<string> Achievments,
        ICollection<CoachReviewWithUserImageResponse> ReviewCoachResponse


        );
}
