

using Club_System_API.Dtos.Appointment;
using Club_System_API.Dtos.Coaches;
using Club_System_API.Dtos.ServiceReview;

namespace Club_System_API.Dtos.Service
{
    public record ServiceWithAllInfoResponse
    (
        int Id,
        string Name,
        decimal Price,
        string Description,
        double AverageRating,
        string? ContentType,
        string? Base64Data,
        ICollection<CoachResponse> coaches,
        ICollection<AppointmentOfServiceResponse> appointments,
        ICollection<ServiceReviewWithUserImageResponse> reviews



        );
}
