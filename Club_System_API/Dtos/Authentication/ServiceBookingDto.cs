using Club_System_API.Models;

namespace Club_System_API.Dtos.Authentication
{
    public record ServiceBookingDto(
        int BookingId,
        string ServiceName,
        DateTime? StartDate,
        BookingStatus Status
    );
}
