namespace Club_System_API.Dtos.ClubReview
{
    public record ClubReviewWithUserImageResponse
    (
          byte[]? Image,
          string FirstName,
          string LastName,
          DateTime ReviewAt,
          int Rating,
          string Review
        );
}
