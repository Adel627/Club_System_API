namespace Club_System_API.Dtos.CoachReview
{
    public record CoachReviewWithUserImageResponse(
         string? ContentType,
        string? Base64Data,
          string FirstName,
          string LastName,
          DateTime ReviewAt,
          int Rating,
          string Review
        );
}
