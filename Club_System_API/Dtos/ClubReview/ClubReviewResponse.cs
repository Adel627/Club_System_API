namespace Club_System_API.Dtos.ClubReview
{
    public record ClubReviewResponse
        (
      string Review,
      int Rating,
      DateTime ReviewAt
        );
}
