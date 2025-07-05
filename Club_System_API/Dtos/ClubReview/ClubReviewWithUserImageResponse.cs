namespace Club_System_API.Dtos.ClubReview
{
    public record ClubReviewWithUserImageResponse
    (
          string? ContentType,
        string? Base64Data,
          string FirstName,
          string LastName,
          DateTime ReviewAt,
          int Rating,
          string Review
        );
}
