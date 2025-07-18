
using Club_System_API.Dtos.Appointment;
using Club_System_API.Dtos.Coaches;
using Club_System_API.Dtos.ServiceReview;
using Club_System_API.Models;

namespace Club_System_API.Dtos.Service
{
    public record ServiceWithAllInfoResponse
    (
        int Id,
        string Name,
        decimal Price,
        string? Bio,
        string Description,
        string? ContentType,
        string? Base64Data,
        ICollection<string> Images,
        double AverageRating,
        ICollection<TimeTableResponse> TimeTable,
        ICollection<ServiceReviewWithUserImageResponse> reviews



        );
}
