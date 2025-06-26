namespace Club_System_API.Dtos.CoachReview
{
    public record CoachReviewWithUserImageResponse(
          byte[]? Image,
          string FirstName,
          string LastName,
          DateTime ReviewAt,
          int Rating,
          string Review
        );
}
