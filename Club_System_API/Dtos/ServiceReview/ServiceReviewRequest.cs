using Club_System_API.Models;

namespace Club_System_API.Dtos.ServiceReview
{
    public  record ServiceReviewRequest(

      int ServiceId  ,
      string Review  ,
      int Rating  
        );
}
