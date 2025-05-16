namespace Club_System_API.Dtos.ServiceReview
{
    public record ServiceReviewResponse(
      int ServiceId,
      string Review,
      int Rating,
      DateTime ReviewAt
        );
    
    
}
