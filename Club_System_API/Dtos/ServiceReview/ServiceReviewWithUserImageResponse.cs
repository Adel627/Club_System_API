namespace Club_System_API.Dtos.ServiceReview
{
    public record ServiceReviewWithUserImageResponse
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
