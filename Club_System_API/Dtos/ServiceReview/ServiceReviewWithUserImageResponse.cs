namespace Club_System_API.Dtos.ServiceReview
{
    public record ServiceReviewWithUserImageResponse
   (
          byte[]? Image,
          string Review,
          int Rating,
          DateTime ReviewAt

        );
}
